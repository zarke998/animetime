package com.example.animetime.utils.htmlembedplayers;

import android.os.CancellationSignal;
import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;

import java.lang.ref.WeakReference;
import java.net.ProtocolException;
import java.util.Timer;
import java.util.TimerTask;

public abstract class HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    protected WeakReference<WebView> mWebViewRef;
    protected boolean mIsPlayActionSimulated = false;
    protected boolean isFullscreen = false;

    public HtmlEmbedPlayerBase(WebView webView){
        mWebViewRef = new WeakReference<WebView>(webView);
    }

    // region IHtmlEmbedPlayer
    @Override
    public void playAsync(Procedure callback) {
        if (!mIsPlayActionSimulated) {
            if(playerHasNewTabAds())
            {
                simulateUserPlayActionRepeteadly(() -> {
                    if (callback != null) callback.run();
                });
            }
            else{
                simulateUserPlayAction();
                if (callback != null) waitPlayerState(EmbedPlayerState.PLAYING ,callback);
            }
            mIsPlayActionSimulated = true;

        } else {
            String playCommand = getFunctionCommand(getPlayCommand());
            injectJavascript(playCommand, value -> {
                if (callback != null) waitPlayerState(EmbedPlayerState.PLAYING ,callback);
            });
        }
    }

    @Override
    public void pauseAsync(Procedure callback) {
        String pauseCommand = getFunctionCommand(getPauseCommand());
        injectJavascript(pauseCommand, value -> {
            if (callback != null) waitPlayerState(EmbedPlayerState.PAUSED, callback);
        });
    }

    @Override
    public int getVideoDurationAsync(ValueCallback<Float> resultCallback) {
        String videoDurationCommand = getFunctionCommand(getVideoDurationCommand());
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

        String videoPosCommand = getFunctionCommand(getVideoPositionCommand());
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
        String seekCommand = getFunctionCommand(getSeekCommand(pos));
        injectJavascript(seekCommand, value -> {
            waitSeekPosChangeAsync(pos, callback);
        });
    }

    @Override
    public void setVolumeAsync(int volumeLevel, Procedure callback) {
        if (volumeLevel < 0) volumeLevel = 0;
        else if (volumeLevel > 100) volumeLevel = 100;
        String volumeSetCommand = getFunctionCommand(getSetVolumeCommand(volumeLevel));
        injectJavascript(volumeSetCommand, value -> {
            if (callback != null) callback.run();
        });
    }

    @Override
    public int getVolumeAsync(ValueCallback<Integer> resultCallback) {
        if (resultCallback == null)
            throw new IllegalArgumentException("Result callback cannot be null.");

        String getVolumeCommand = getFunctionCommand(getGetVolumeCommand());
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
        if(isFullscreenProtected()) return;

        String fullscreenCommand = getFunctionCommand(getSetFullscreenCommand(fullscreen));
        injectJavascript(fullscreenCommand, value -> {
            if (callback != null) {
                waitFullscreenChangeAsync(!fullscreen, callback);
            }
        });
    }

    public abstract boolean isFullscreenProtected();

    @Override
    public void getFullscreenAsync(ValueCallback<Boolean> resultCallback) {
        String getFullscreenCommand = getFunctionCommand(getGetFullscreenCommand());
        injectJavascript(getFullscreenCommand, value -> {
            if (value == "true") resultCallback.onReceiveValue(true);
            else if (value == "false") resultCallback.onReceiveValue(false);
        });
    }

    @Override
    public void hidePlayerControlsAsync(Procedure callback) {
        String command = getFunctionCommand(getHideControlsCommand());
        injectJavascript(command, value -> {
            if(callback != null) callback.run();
        });
    }

    // endregion
    // region Injecting javascript
    protected void injectJavascript(String javascript, ValueCallback<String> callback){
        WebView webView = mWebViewRef.get();
        if(webView != null){
            webView.evaluateJavascript(javascript, value -> {
                if(callback != null) callback.onReceiveValue(value);
            });
    }
    }
    protected void injectJavascript(String javascript){
        injectJavascript(javascript, null);
    }
    protected String getFunctionCommand(String commandBody){
        return String.format("(function x(){%s})()", commandBody);
    }
    // endregion

    // Helper methods
    protected Float[] htmlElementPosToWebViewPos(float htmlWindowWidth, float htmlWindowHeight, float elWidth, float elHeight) {
        WebView webView = mWebViewRef.get();

        Float[] result = new Float[2];
        result[0] = elWidth * (webView.getWidth() / htmlWindowWidth);
        result[1] = elHeight * (webView.getHeight() / htmlWindowHeight);

        return result;
    }

    // region Waiting for player to get into a state
    protected void waitFullscreenChangeAsync(boolean originalValue, Procedure callback) {
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
    protected void waitSeekPosChangeAsync(int targetPos, Procedure callback) {
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
    /** Waits for player to enter targetState, then executes callback on UI thread.
     * @param targetState
     * @param callback
     */
    protected void waitPlayerState(EmbedPlayerState targetState, Procedure callback){
        Timer t = new Timer();
        TimerTask tTask = new TimerTask() {
            int runCount = 0;
            boolean isChecking = false;
            @Override
            public void run() {
                if(runCount > 150){
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
    // endregion
    // region User simulation
    protected abstract void simulateUserPlayAction();
    public abstract void simulateUserFullscreenAction();
    /** Simulate user play action repeteadly and execute callback on UI thread when player gets in
     * playing state.
     * @param callback
     */
    private void simulateUserPlayActionRepeteadly(Procedure callback){
        Timer t = new Timer();
        TimerTask tTask = new TimerTask() {
            int runCount;
            boolean callbackCalled = false;

            @Override
            public void run() {
                if(mWebViewRef.get() == null && runCount > 10){
                    t.cancel();
                    return;
                }
                mWebViewRef.get().post(() -> {
                    simulateUserPlayAction();
                    waitPlayerState(EmbedPlayerState.PLAYING, () -> {
                        t.cancel();
                        if(callback != null && callbackCalled == false){
                            callbackCalled = true;
                            callback.run();
                        }
                    });
                });

                runCount++;
            }
        };
        t.schedule(tTask, 0, 500);
    }
    // endregion
    // region Custom properties
    protected abstract boolean isAlreadyFullscreened();
    protected abstract boolean playerHasNewTabAds();
    // endregion
    // region Player commands
    protected abstract String getPlayCommand();
    protected abstract String getPauseCommand();
    protected abstract String getVideoDurationCommand();
    protected abstract String getVideoPositionCommand();
    protected abstract String getSeekCommand(int pos);

    /** Get setVolumeCommand.
     * @param volumeLevel Range 0-100.
     * @return
     */
    protected abstract String getSetVolumeCommand(int volumeLevel);
    protected abstract String getGetVolumeCommand();
    protected abstract String getGetFullscreenCommand();
    protected abstract String getSetFullscreenCommand(boolean fullscreen);
    protected abstract String getHideControlsCommand();
    // endregion
}