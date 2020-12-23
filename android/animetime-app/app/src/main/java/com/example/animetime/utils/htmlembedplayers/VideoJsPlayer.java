package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;
import com.example.animetime.utils.ViewExtensions;
import com.example.animetime.utils.values.BooleanExtensions;

public class VideoJsPlayer extends HtmlEmbedPlayerBase {
    public VideoJsPlayer(WebView webView) {
        super(webView);
    }

    @Override
    public void getPlayerStateAsync(ValueCallback<EmbedPlayerState> resultCallback) {
        String isPausedCommand = getFunctionCommand("return player.paused();");
        String isSeekingCommand = getFunctionCommand("return player.seeking();");

        injectJavascript(isPausedCommand, isPauseBoolStr -> {
            Boolean isPaused = BooleanExtensions.tryGetBoolFromStr(isPauseBoolStr);
            if (isPaused == null) {
                // Log js change
                resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                return;
            }
            injectJavascript(isSeekingCommand, isSeekingBoolStr -> {
                Boolean isSeeking = BooleanExtensions.tryGetBoolFromStr(isSeekingBoolStr);
                if (isSeekingBoolStr == null) {
                    //Log js change
                    resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                    return;
                }

                if (isPaused) resultCallback.onReceiveValue(EmbedPlayerState.PAUSED);
                else if (isSeeking) resultCallback.onReceiveValue(EmbedPlayerState.SEEKING);
                else resultCallback.onReceiveValue(EmbedPlayerState.PLAYING);

                return;
            });

        });

    }

    @Override
    protected void simulateUserPlayAction() {
        showBigPlayButtonAsync(() -> {
            if(mWebViewRef.get() != null)
                ViewExtensions.simulateTouchOnCenter(mWebViewRef.get());
        });
    }

    @Override
    public void simulateUserFullscreenAction() {
        return;
    }

    @Override
    public boolean isFullscreenProtected() {
        return true;
    }

    @Override
    protected boolean isAlreadyFullscreened() {
        return true;
    }

    @Override
    protected boolean playerHasNewTabAds() {
        return false;
    }

    @Override
    protected boolean isVolumeMax100() {
        return false;
    }

    // region Player commands
    @Override
    protected String getPlayCommand() {
        return "player.play();";
    }

    @Override
    protected String getPauseCommand() {
        return "player.pause();";
    }

    @Override
    protected String getVideoDurationCommand() {
        return "return player.duration();";
    }

    @Override
    protected String getVideoPositionCommand() {
        return "return player.currentTime();";
    }

    @Override
    protected String getSeekCommand(int pos) {
        return String.format("player.currentTime(%s);",  pos);
    }

    // Volume range 0.0 -> 1.0
    @Override
    protected String getSetVolumeCommand(int volumeLevel) {
        return String.format("player.volume(%s);", volumeLevel / 100.0f);
    }

    @Override
    protected String getGetVolumeCommand() {
        return "return player.volume();";
    }

    @Override
    protected String getGetFullscreenCommand() {
        return "return player.isFullscreen_;";
    }

    @Override
    protected String getSetFullscreenCommand(boolean fullscreen) {
        return fullscreen ? "player.requestFullscreen();" : "player.exitFullscreen();";
    }

    @Override
    protected String getHideControlsCommand() {
        return "$(\".vjs-control-bar\").hide();" +
                "$(\".vjs-big-play-button\").hide();" +
                "$(\".vjs-loading-spinner\").hide();";
    }

    // endregion

    private void showBigPlayButtonAsync(Procedure callback){
        String showBtnCommand = getFunctionCommand("$(\".vjs-big-play-button\").show();");
        injectJavascript(showBtnCommand, value -> {
            if(callback != null) callback.run();
        });
    }
}
