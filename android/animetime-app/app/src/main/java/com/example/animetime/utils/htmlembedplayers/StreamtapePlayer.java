package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.ViewExtensions;
import com.example.animetime.utils.values.BooleanExtensions;

public class StreamtapePlayer extends HtmlEmbedPlayerBase {
    public StreamtapePlayer(WebView webView) {
        super(webView);
    }

    @Override
    public void getPlayerStateAsync(ValueCallback<EmbedPlayerState> resultCallback) {
        String playingCommand = getFunctionCommand("return player.playing");
        String pausedCommand = getFunctionCommand("return player.paused");
        String seekingCommand = getFunctionCommand("return player.seeking");

        injectJavascript(playingCommand, isPlayingStr -> {
            Boolean isPlaying = BooleanExtensions.tryGetBoolFromStr(isPlayingStr);
            if(isPlaying == null){
                resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                return;
            }

            injectJavascript(pausedCommand, isPausedStr -> {
                Boolean isPaused = BooleanExtensions.tryGetBoolFromStr(isPausedStr);
                if(isPaused == null){
                    resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                    return;
                }

                injectJavascript(seekingCommand, isSeekingStr -> {
                    Boolean isSeeking = BooleanExtensions.tryGetBoolFromStr(isSeekingStr);
                    if(isSeeking == null){
                        resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                        return;
                    }

                    if(isPlaying) resultCallback.onReceiveValue(EmbedPlayerState.PLAYING);
                    else if(isPaused) resultCallback.onReceiveValue(EmbedPlayerState.PAUSED);
                    else if(isSeeking) resultCallback.onReceiveValue(EmbedPlayerState.SEEKING);
                    else resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                });
            });
        });
    }

    @Override
    protected void simulateUserPlayAction() {
        if(mWebViewRef.get() != null)
            ViewExtensions.simulateTouchOnCenter(mWebViewRef.get());
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
        return true;
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
        return "return player.duration;";
    }

    @Override
    protected String getVideoPositionCommand() {
        return "return player.currentTime;";
    }

    @Override
    protected String getSeekCommand(int pos) {
        return String.format("player.currentTime = %s", pos);
    }

    //Uses range 0.0 - 1.0
    @Override
    protected String getSetVolumeCommand(int volumeLevel) {
        if(volumeLevel < 0) volumeLevel = 0;
        else if(volumeLevel > 100) volumeLevel = 100;

        float volumeLevelFloat = volumeLevel / 100.0f;

        return String.format("player.volume = %s", volumeLevelFloat);
    }

    @Override
    protected String getGetVolumeCommand() {
        return "return player.volume";
    }

    @Override
    protected String getGetFullscreenCommand() {
        return "player.fullscreen.active";
    }

    @Override
    protected String getSetFullscreenCommand(boolean fullscreen) {
        return fullscreen ? "player.fullscreen.enter();" : "player.fullscreen.exit();";
    }

    @Override
    protected String getHideControlsCommand() {
        return "document.querySelector(\".plyr__controls\").style = \"display: none\";\n" +
                "document.querySelector(\".plyr__anim\").style = \"display: none\";";
    }
    // endregion
}
