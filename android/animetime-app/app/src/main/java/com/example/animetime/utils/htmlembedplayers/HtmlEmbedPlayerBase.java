package com.example.animetime.utils.htmlembedplayers;

import android.webkit.ValueCallback;
import android.webkit.WebView;

import java.lang.ref.WeakReference;

public abstract class HtmlEmbedPlayerBase implements IHtmlEmbedPlayer{
    protected WeakReference<WebView> mWebViewRef;

    public HtmlEmbedPlayerBase(WebView webView){
        mWebViewRef = new WeakReference<WebView>(webView);
    }

    protected void injectJavascript(String javascript, ValueCallback<String> callback){
        WebView webView = mWebViewRef.get();
        if(webView != null){
            webView.evaluateJavascript(javascript, value -> {
                if(callback != null) callback.onReceiveValue(value);
            });
        }
    }
    protected void injectJavascript(String javascript){
        injectJavascript(javascript, null);
    }
    protected String getFunctionCommand(String commandBody){
        return String.format("(function x(){%s})()", commandBody);
    }
}

