package com.example.animetime.utils.htmlembedplayers;

import android.util.Pair;
import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;
import com.example.animetime.utils.ViewExtensions;

public class MixdropPlayer extends HtmlEmbedPlayerBase {
    public MixdropPlayer(WebView webView) {
        super(webView);
    }

    @Override
    protected void simulateUserPlayAction() {
        if (mWebViewRef.get() != null) {
            ViewExtensions.simulateTouchOnCenter(mWebViewRef.get());
        }
    }

    @Override
    public void simulateUserFullscreenAction() {
        String fullscreenBtnCommand = getFunctionCommand("var playBtnRect = $(\".vjs-fullscreen-control]\")[0].getBoundingClientRect(); return playBtnRect.x + ',' + playBtnRect.y;");
        String windowDimensionsCommand = getFunctionCommand("return window.innerWidth + ',' + window.innerHeight;");
        injectJavascript(fullscreenBtnCommand, fullscreenBtnString -> {
            String[] fullscreenBtnPos = fullscreenBtnString.replace("\"", "").split(",");
            injectJavascript(windowDimensionsCommand, winDimString -> {
                String[] windowDimensions = winDimString.replace("\"", "").split(",");
                Float[] elPosOnWebView = htmlElementPosToWebViewPos(
                        Float.parseFloat(windowDimensions[0]),
                        Float.parseFloat(windowDimensions[1]),
                        Float.parseFloat(fullscreenBtnPos[0]),
                        Float.parseFloat(fullscreenBtnPos[1])
                );
                int positionOffset = 5;
                ViewExtensions.simulateTouch(mWebViewRef.get(), elPosOnWebView[0] + positionOffset, elPosOnWebView[1] + positionOffset);
            });
        });
    }

    @Override
    protected boolean playerHasNewTabAds() {
        return true;
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
    public void getPlayerState(ValueCallback<EmbedPlayerState> resultCallback) {
        String isPausedCommand = getFunctionCommand("return MDCore.player.paused();");
        String isSeekingCommand = getFunctionCommand("return MDCore.player.seeking();");

        injectJavascript(isPausedCommand, isPauseBoolStr -> {
            Boolean isPaused = tryGetBoolFromStr(isPauseBoolStr);
            if (isPaused == null) {
                // Log js change
                resultCallback.onReceiveValue(EmbedPlayerState.NONE);
                return;
            }
            injectJavascript(isSeekingCommand, isSeekingBoolStr -> {
                Boolean isSeeking = tryGetBoolFromStr(isSeekingBoolStr);
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

    // region Player commands
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
        if (fullscreen) return "MDCore.player.requestFullscreen();";
        else return "MDCore.player.exitFullscreen();";
    }

    // endregion

    // Helper methods
    private Boolean tryGetBoolFromStr(String stringedBool) {
        if (stringedBool == null) return null;

        stringedBool = stringedBool.replace("\"", "");

        if (stringedBool.equals("true")) return true;
        else if (stringedBool.equals("false")) return false;
        else return null;
    }
}
