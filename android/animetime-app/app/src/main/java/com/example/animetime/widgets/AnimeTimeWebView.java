package com.example.animetime.widgets;

import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.ConsoleMessage;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import org.adblockplus.libadblockplus.android.webview.AdblockWebView;

public class AnimeTimeWebView extends AdblockWebView {
    private WebView mWebView;

    public AnimeTimeWebView(Context context) {
        super(context);
        mWebView = this;

        configureWebView();
    }
    public AnimeTimeWebView(Context context, AttributeSet attrs) {
        super(context, attrs);
        mWebView = this;

        configureWebView();
    }
    public AnimeTimeWebView(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
        mWebView = this;

        configureWebView();
    }

    private void configureWebView(){
        this.getSettings().setJavaScriptEnabled(true);
        this.getSettings().setUserAgentString("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");

        this.setWebViewClient(new WebViewClient(){
            @Override
            public void onPageFinished(WebView view, String url) {
                super.onPageFinished(view, url);
            }
        });
        this.setWebChromeClient(new WebChromeClient(){
            private View mFullscreenView;
            private CustomViewCallback mCustomCallback;

            @Override
            public boolean onConsoleMessage(ConsoleMessage consoleMessage) {
                super.onConsoleMessage(consoleMessage);

                return true;
            }

            @Override
            public void onShowCustomView(View view, CustomViewCallback callback) {
                if(mFullscreenView != null){
                    callback.onCustomViewHidden();
                    return;
                }

                mFullscreenView = view;
                mFullscreenView.setLayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
                mFullscreenView.setZ(-1);

                mWebView.setVisibility(View.GONE);

                ViewGroup rootLayout = (ViewGroup)mWebView.getRootView();
                rootLayout.addView(mFullscreenView);

                mCustomCallback = callback;
            }

            @Override
            public void onHideCustomView() {
                super.onHideCustomView();
                if(mFullscreenView == null){
                    return;
                }

                mWebView.setVisibility(View.VISIBLE);
                mFullscreenView.setVisibility(View.GONE);

                ViewGroup rootLayout = (ViewGroup)mWebView.getRootView();
                rootLayout.removeView(mFullscreenView);

                mCustomCallback.onCustomViewHidden();
                mFullscreenView = null;
            }
        });
    }
}
