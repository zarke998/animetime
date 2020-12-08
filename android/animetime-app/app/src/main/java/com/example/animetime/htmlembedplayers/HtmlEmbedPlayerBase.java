package com.example.animetime.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

public abstract class HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    protected WebView mWebView;
    protected HtmlEmbedPlayerStateListener mPlayerStateListener;

    public HtmlEmbedPlayerBase(WebView webView){
        mWebView = webView;
    }

    protected void injectJavascript(String javascript, ValueCallback<String> callback){
        mWebView.evaluateJavascript(javascript, value -> {
            if(callback != null) callback.onReceiveValue(value);
        });
    }

    protected void injectJavascript(String javascript){
        injectJavascript(javascript, null);
    }

    public class HtmlEmbedPlayerStateListener {
        public void onPlayExecuted(){}
        public void onPauseExectued(){}
        public void onSeekExecuted(){}
    }
}

