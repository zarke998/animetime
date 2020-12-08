package com.example.animetime.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

public class JWPlayer extends HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    public JWPlayer(WebView webView) {
        super(webView);
    }

    @Override
    public void play() {
        String playCommand = "(function play(){jwplayer().play();})()";
        injectJavascript(playCommand, value -> {
            if(mPlayerStateListener != null) mPlayerStateListener.onPlayExecuted();
        });
    }
    @Override
    public void pause() {
        String pauseCommand = "(function pause(){jwplayer().pause();})()";
        injectJavascript(pauseCommand, value -> {
            if(mPlayerStateListener != null) mPlayerStateListener.onPauseExectued();
        });
    }
    @Override
    public int getVideoDuration(ValueCallback<Float> resultCallback) {
        String videoDurationCommand = "(function videoDuration(){ return jwplayer().getDuration();})()";
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
    public void seek(int pos) {
        String seekCommand = String.format("(function seek(){jwplayer().seek(%s);})()", pos);
        injectJavascript(seekCommand, value -> {
            if(mPlayerStateListener != null) mPlayerStateListener.onSeekExecuted();
        });
    }
}
