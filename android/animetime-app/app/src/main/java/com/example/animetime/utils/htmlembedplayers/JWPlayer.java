package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import com.example.animetime.utils.Procedure;
import com.example.animetime.utils.ViewExtensions;

import java.util.Timer;
import java.util.TimerTask;

public class JWPlayer extends HtmlEmbedPlayerBase implements IHtmlEmbedPlayer {
    public JWPlayer(WebView webView) {
        super(webView);
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
    protected void simulateUserPlayAction() {

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

    @Override
    protected boolean playerHasNewTabAds() {
        return false;
    }

    //Player commands
    @Override
    protected String getPlayCommand() {
        return "jwplayer().play();";
    }

    @Override
    protected String getPauseCommand() {
        return "jwplayer().pause();";
    }

    @Override
    protected String getVideoDurationCommand() {
        return "return jwplayer().getDuration();";
    }

    @Override
    protected String getVideoPositionCommand() {
        return "return jwplayer().getPosition();";
    }

    @Override
    protected String getSeekCommand(int pos) {
        return String.format("jwplayer().seek(%s);", pos);
    }

    @Override
    protected String getSetVolumeCommand(int volumeLevel) {
        return String.format("jwplayer().setVolume(%s);", volumeLevel);
    }

    @Override
    protected String getGetVolumeCommand() {
        return "return jwplayer().getVolume();";
    }

    @Override
    protected String getGetFullscreenCommand() {
        return "jwplayer().getFullscreen();";
    }

    @Override
    protected String getSetFullscreenCommand(boolean fullscreen) {
        String jsParam = fullscreen ? "true" : "false";

        return String.format("jwplayer().setFullscreen(%s)", jsParam);
    }
}