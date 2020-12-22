package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;

import com.example.animetime.utils.Procedure;

interface IHtmlEmbedPlayer {
    void playAsync(Procedure callback);
    void pauseAsync(Procedure callback);
    void getPlayerState(ValueCallback<EmbedPlayerState> resultCallback);

    int getVideoDurationAsync(ValueCallback<Float> resultCallback);
    int getVideoPositionAsync(ValueCallback<Float> resultCallback);

    void seekAsync(int pos, Procedure callback);

    /** Set player volume
     * @param volumeLevel Range 0-100.
     * @param callback
     */
    void setVolumeAsync(int volumeLevel, Procedure callback);
    int getVolumeAsync(ValueCallback<Integer> resultCallback);

    void setFullscreenAsync(boolean fullsreen, Procedure callback);
    void getFullscreenAsync(ValueCallback<Boolean> resultCallback);
    boolean isFullscreenProtected();

    void hidePlayerControlsAsync(Procedure callback);
    void setup(Procedure callback);
}