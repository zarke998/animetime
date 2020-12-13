package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;

import com.example.animetime.utils.Procedure;

interface IHtmlEmbedPlayer {
    void playAsync(Procedure callback);
    void pauseAsync(Procedure callback);
    void isPlayingAsync(ValueCallback<Boolean> resultCallback);
    void getPlayerState(ValueCallback<EmbedPlayerState> resultCallback);

    int getVideoDurationAsync(ValueCallback<Float> resultCallback);
    int getVideoPositionAsync(ValueCallback<Float> resultCallback);

    void seekAsync(int pos, Procedure callback);

    void setVolumeAsync(int volumeLevel, Procedure callback);
    int getVolumeAsync(ValueCallback<Integer> resultCallback);

    void setFullscreenAsync(boolean fullsreen, Procedure callback);
    void getFullscreenAsync(ValueCallback<Boolean> resultCallback);
}