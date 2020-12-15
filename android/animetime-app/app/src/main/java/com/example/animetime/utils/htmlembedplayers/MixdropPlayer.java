package com.example.animetime.utils.htmlembedplayers;

import android.util.Pair;
import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;

public class MixdropPlayer extends HtmlEmbedPlayerBase{
    public MixdropPlayer(WebView webView) {
        super(webView);
    }

    @Override
    protected void simulateUserPlayAction() {

    }

    @Override
    protected String getPlayCommand() {
        return "MDCore.player.play();";
    }

    @Override
    protected String getPauseCommand() {
        return "MDCore.player.pause();";
    }

    @Override
    protected String getVideoDurationCommand() {
        return "return MDCore.player.duration();";
    }

    @Override
    protected String getVideoPositionCommand() {
        return "return MDCore.player.currentTime();";
    }

    @Override
    protected String getSeekCommand(int pos) {
        return String.format("MDCore.player.tech_.setCurrentTime(%s)", pos);
    }

    @Override
    protected String getSetVolumeCommand(int volumeLevel) {
        float scaledVolumeLvl = volumeLevel / 100.0f;
        return String.format("MDCore.player.tech_.setVolume(%s)", scaledVolumeLvl);
    }

    @Override
    protected String getGetVolumeCommand() {
        return "return MDCore.player.tech_.volume()";
    }

    @Override
    protected String getGetFullscreenCommand() {
        return "return MDCore.player.isFullscreen_";
    }

    @Override
    protected String getSetFullscreenCommand(boolean fullscreen) {
        if(fullscreen) return "MDCore.player.requestFullscreen();";
        else return "MDCore.player.exitFullscreen();";
    }

    @Override
    public void getPlayerState(ValueCallback<EmbedPlayerState> resultCallback) {
        String isPausedCommand = getFunctionCommand("return MDCore.player.paused();");
        String isSeekingCommand = getFunctionCommand("return MDCore.player.seeking();");

        injectJavascript(isPausedCommand, isPauseBoolStr -> {
            Boolean isPaused = tryGetBoolFromStr(isPauseBoolStr);
            if(isPaused == null){
                // Log js change
                resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                return;
            }
            injectJavascript(isSeekingCommand, isSeekingBoolStr -> {
                Boolean isSeeking = tryGetBoolFromStr(isSeekingBoolStr);
                if(isSeekingBoolStr == null){
                    //Log js change
                    resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                    return;
                }

                if(isPaused) resultCallback.onReceiveValue(EmbedPlayerState.PAUSED);
                else if(isSeeking) resultCallback.onReceiveValue(EmbedPlayerState.SEEKING);
                else resultCallback.onReceiveValue(EmbedPlayerState.PLAYING);

                return;
            });

        });
    }

    private Boolean tryGetBoolFromStr(String stringedBool){
        if(stringedBool == null) return null;

        stringedBool = stringedBool.replace("\"","");

        if(stringedBool.equals("true")) return true;
        else if(stringedBool.equals("false")) return false;
        else return null;
    }
}
