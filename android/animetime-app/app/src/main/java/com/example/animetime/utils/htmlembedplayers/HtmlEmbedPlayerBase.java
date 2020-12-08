package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

public abstract class HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    protected WebView mWebView;

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
    protected String getFunctionCommand(String commandBody){
        return String.format("(function x(){%s})()", commandBody);
    }
}

