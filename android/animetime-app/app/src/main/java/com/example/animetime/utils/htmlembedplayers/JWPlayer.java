package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;
import com.example.animetime.utils.ViewExtensions;

import java.util.Timer;
import java.util.TimerTask;

public class JWPlayer extends HtmlEmbedPlayerBase implements IHtmlEmbedPlayer {
    private boolean mIsPlayActionSimulated = false;
    public boolean isFullscreen = false;
    private JWPlayer mJWPlayer;

    public JWPlayer(WebView webView) {
        super(webView);
        mJWPlayer = this;
    }

    @Override
    public void playAsync(Procedure callback) {
        if (!mIsPlayActionSimulated) {
            simulateUserPlayAction();
            mIsPlayActionSimulated = true;

            if (callback != null) waitPlayerState(EmbedPlayerState.PLAYING ,callback);
        } else {
            String playCommand = getFunctionCommand("jwplayer().play();");
            injectJavascript(playCommand, value -> {
                if (callback != null) waitPlayerState(EmbedPlayerState.PLAYING ,callback);
            });
        }
    }

    @Override
    public void pauseAsync(Procedure callback) {
        String pauseCommand = getFunctionCommand("jwplayer().pause();");
        injectJavascript(pauseCommand, value -> {
            if (callback != null) waitPlayerState(EmbedPlayerState.PAUSED, callback);
        });
    }

    @Override
    public void getPlayerState(ValueCallback<EmbedPlayerState> resultCallback) {
        String playerStateCommand = getFunctionCommand("return jwplayer().getState();");
        injectJavascript(playerStateCommand, value -> {
            if (value == null) resultCallback.onReceiveValue(EmbedPlayerState.NONE);

            value = value.replace("\"","");
            switch (value) {
                case "playing":
                    resultCallback.onReceiveValue(EmbedPlayerState.PLAYING);
                    break;
                case "paused":
                    resultCallback.onReceiveValue(EmbedPlayerState.PAUSED);
                    break;
                case "buffering":
                    resultCallback.onReceiveValue(EmbedPlayerState.SEEKING);
                    break;
                default:
                    resultCallback.onReceiveValue(EmbedPlayerState.NONE);
            }
        });

    }

    @Override
    public int getVideoDurationAsync(ValueCallback<Float> resultCallback) {
        String videoDurationCommand = getFunctionCommand("return jwplayer().getDuration();");
        injectJavascript(videoDurationCommand, value -> {
            if (value == null) {
                resultCallback.onReceiveValue(0f);
                return;
            }

            float result = 0f;
            try {
                result = Float.parseFloat(value);
            } catch (NumberFormatException formatException) {
            }
            resultCallback.onReceiveValue(result);

            return;
        });

        return 0;
    }

    @Override
    public int getVideoPositionAsync(ValueCallback<Float> resultCallback) {
        if (resultCallback == null)
            throw new IllegalArgumentException("Result callback cannot be null.");

        String videoPosCommand = getFunctionCommand("return jwplayer().getPosition();");
        injectJavascript(videoPosCommand, value -> {
            try {
                float result = Float.parseFloat(value);
                resultCallback.onReceiveValue(result);
            } catch (Exception e) {
                resultCallback.onReceiveValue(0.0f);
            }
        });

        return 0;
    }

    @Override
    public void seekAsync(int pos, Procedure callback) {
        String seekCommand = getFunctionCommand(String.format("jwplayer().seek(%s);", pos));
        injectJavascript(seekCommand, value -> {
            waitSeekPosChangeAsync(pos, callback);
        });
    }

    @Override
    public void setVolumeAsync(int volumeLevel, Procedure callback) {
        if (volumeLevel < 0) volumeLevel = 0;
        else if (volumeLevel > 100) volumeLevel = 100;

        String volumeSetCommand = getFunctionCommand(String.format("jwplayer().setVolume(%s);", volumeLevel));
        injectJavascript(volumeSetCommand, value -> {
            if (callback != null) callback.run();
        });
    }

    @Override
    public int getVolumeAsync(ValueCallback<Integer> resultCallback) {
        if (resultCallback == null)
            throw new IllegalArgumentException("Result callback cannot be null.");

        String getVolumeCommand = getFunctionCommand(String.format("return jwplayer().getVolume();"));
        injectJavascript(getVolumeCommand, value -> {
            try {
                int result = Integer.parseInt(value);
                resultCallback.onReceiveValue(result);
            } catch (NumberFormatException exception) {
                resultCallback.onReceiveValue(0);
            }
        });

        return 0;
    }

    @Override
    public void setFullscreenAsync(boolean fullscreen, Procedure callback) {
        String jsParam = fullscreen ? "true" : "false";

        String fullscreenCommand = getFunctionCommand(String.format("jwplayer().setFullscreen(%s)", jsParam));
        injectJavascript(fullscreenCommand, value -> {
            if (callback != null) {
                waitFullscreenChangeAsync(!fullscreen, callback);
            }
        });
    }

    @Override
    public void getFullscreenAsync(ValueCallback<Boolean> resultCallback) {
        String getFullscreenCommand = getFunctionCommand("jwplayer().getFullscreen();");
        injectJavascript(getFullscreenCommand, value -> {
            if (value == "true") resultCallback.onReceiveValue(true);
            else if (value == "false") resultCallback.onReceiveValue(false);
        });
    }

    private void simulateUserPlayAction() {

        String playBtnPosCommand = getFunctionCommand("var playBtnRect = $(\"div[aria-label='Play']\")[0].getBoundingClientRect(); return playBtnRect.x + ',' + playBtnRect.y;");
        String windowDimensionsCommand = getFunctionCommand("return window.innerWidth + ',' + window.innerHeight;");
        injectJavascript(playBtnPosCommand, playBtnString -> {
            String[] playBtnPos = playBtnString.replace("\"", "").split(",");
            injectJavascript(windowDimensionsCommand, winDimString -> {
                String[] windowDimensions = winDimString.replace("\"", "").split(",");
                Float[] elPosOnWebView = htmlElementPosToWebViewPos(
                        Float.parseFloat(windowDimensions[0]),
                        Float.parseFloat(windowDimensions[1]),
                        Float.parseFloat(playBtnPos[0]),
                        Float.parseFloat(playBtnPos[1])
                );
                int positionOffset = 5;
                ViewExtensions.simulateTouch(mWebViewRef.get(), elPosOnWebView[0] + positionOffset, elPosOnWebView[1] + positionOffset);
            });
        });
    }
    private Float[] htmlElementPosToWebViewPos(float htmlWindowWidth, float htmlWindowHeight, float elWidth, float elHeight) {
        WebView webView = mWebViewRef.get();

        Float[] result = new Float[2];
        result[0] = elWidth * (webView.getWidth() / htmlWindowWidth);
        result[1] = elHeight * (webView.getHeight() / htmlWindowHeight);

        return result;
    }
    private void waitFullscreenChangeAsync(boolean originalValue, Procedure callback) {
        if (callback == null) return;

        Timer t = new Timer();
        TimerTask tTask = new TimerTask() {
            boolean fullscreenIsChecking = false;
            int runCount = 0;

            @Override
            public void run() {
                if (runCount > 30) {
                    t.cancel();
                    return;
                }
                WebView webView = mWebViewRef.get();
                if (webView == null) {
                    t.cancel();
                    return;
                }
                if (!fullscreenIsChecking) {
                    webView.post(() -> {
                        getFullscreenAsync(value ->
                        {
                            if (originalValue != value) {
                                isFullscreen = value;
                                callback.run();
                            }

                            fullscreenIsChecking = false;
                        });
                    });
                }
                fullscreenIsChecking = true;

                runCount++;
            }
        };

        t.schedule(tTask, 0, 100);
    }
    private void waitSeekPosChangeAsync(int targetPos, Procedure callback) {
        if (callback == null) return;

        Timer t = new Timer();
        TimerTask tTask = new TimerTask() {
            int runCount = 0;
            boolean posCheckIsProcessing = false;

            @Override
            public void run() {
                runCount++;
                if (runCount > 150) {
                    t.cancel();
                    return;
                }

                if (!posCheckIsProcessing && mWebViewRef.get() != null) {
                    mWebViewRef.get().post(() -> {
                        getPlayerState(state -> {
                            if (state != EmbedPlayerState.SEEKING) {
                                if (callback != null) {
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
    }
    private void waitPlayerState(EmbedPlayerState targetState, Procedure callback){
        Timer t = new Timer();
        TimerTask tTask = new TimerTask() {
            int runCount = 0;
            boolean isChecking = false;
            @Override
            public void run() {
                if(runCount > 1500){
                    t.cancel();
                    return;
                }

                if(!isChecking && mWebViewRef.get() != null){
                    mWebViewRef.get().post(() -> {
                        getPlayerState(state -> {
                            if(state == targetState){
                                callback.run();
                                t.cancel();
                            }
                            isChecking = false;
                        });
                    });
                }
                isChecking = true;
            }
        };
        t.schedule(tTask, 0, 100);
    }
}