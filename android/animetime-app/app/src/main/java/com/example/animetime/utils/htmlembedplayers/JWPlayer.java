package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;

import java.util.Timer;
import java.util.TimerTask;

public class JWPlayer extends HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    public JWPlayer(WebView webView) {
        super(webView);
    }

    @Override
    public void playAsync(Procedure callback) {
        String playCommand = getFunctionCommand("jwplayer().play();");
        injectJavascript(playCommand, value -> {
            if(callback != null) callback.run();
        });
    }
    @Override
    public void pauseAsync(Procedure callback) {
        String pauseCommand = getFunctionCommand("jwplayer().pause();");
        injectJavascript(pauseCommand, value -> {
            if(callback != null) callback.run();
        });
    }
    @Override
    public int getVideoDurationAsync(ValueCallback<Float> resultCallback) {
        String videoDurationCommand = getFunctionCommand("return jwplayer().getDuration();");
        injectJavascript(videoDurationCommand, value -> {
            if(value == null) {
                resultCallback.onReceiveValue(0f);
                return;
            }

            float result = 0f;
            try{
                result = Float.parseFloat(value);
            }catch (NumberFormatException formatException){}
            resultCallback.onReceiveValue(result);

            return;
        });

        return 0;
    }
    @Override
    public int getVideoPositionAsync(ValueCallback<Float> resultCallback) {
        if(resultCallback == null) throw new IllegalArgumentException("Result callback cannot be null.");

        String videoPosCommand = getFunctionCommand("return jwplayer().getPosition();");
        injectJavascript(videoPosCommand, value -> {
            try{
                float result = Float.parseFloat(value);
                resultCallback.onReceiveValue(result);
            }
            catch (Exception e){ resultCallback.onReceiveValue(0.0f); }
        });

        return 0;
    }
    @Override
    public void seekAsync(int pos, Procedure callback) {
        String seekCommand = getFunctionCommand(String.format("jwplayer().seek(%s);", pos));
        injectJavascript(seekCommand, value -> {

            Timer t = new Timer();
            TimerTask tTask = new TimerTask(){
                int runCount = 0;
                boolean posCheckIsProcessing = false;

                @Override
                public void run() {
                    runCount++;
                    if(runCount > 30) {
                        t.cancel();
                        return;
                    }

                    if(!posCheckIsProcessing && mWebViewRef.get() != null){
                        mWebViewRef.get().post(() -> {
                            getVideoPositionAsync(position -> {
                                if(position >= pos){
                                    if(callback != null){
                                        callback.run();
                                    }
                                    t.cancel();
                                }
                                posCheckIsProcessing = false;
                            });
                        });
                    }
                    posCheckIsProcessing = true;
                }
            };
            t.schedule(tTask, 0, 100);
        });
    }
    @Override
    public void setVolumeAsync(int volumeLevel, Procedure callback) {
        if(volumeLevel < 0) volumeLevel = 0;
        else if(volumeLevel > 100) volumeLevel = 100;

        String volumeSetCommand = getFunctionCommand(String.format("jwplayer().setVolume(%s);", volumeLevel));
        injectJavascript(volumeSetCommand, value -> {
            if(callback != null) callback.run();
        });
    }
    @Override
    public int getVolumeAsync(ValueCallback<Integer> resultCallback) {
        if(resultCallback == null) throw new IllegalArgumentException("Result callback cannot be null.");

        String getVolumeCommand = getFunctionCommand(String.format("return jwplayer().getVolume();"));
        injectJavascript(getVolumeCommand, value -> {
            try{
                int result = Integer.parseInt(value);
                resultCallback.onReceiveValue(result);
            }
            catch(NumberFormatException exception){ resultCallback.onReceiveValue(0);}
        });

        return 0;
    }
}