package com.example.animetime.htmlembedplayers;

import android.webkit.ValueCallback;

public interface IHtmlEmbedPlayer {
    void play();
    void pause();
    int getVideoDuration(ValueCallback<Float> resultCallback);
    void seek(int pos);
}