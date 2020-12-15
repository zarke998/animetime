package com.example.animetime;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.ConsoleMessage;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.RelativeLayout;


import com.example.animetime.utils.htmlembedplayers.JWPlayer;

import org.adblockplus.libadblockplus.android.webview.AdblockWebView;

public class MainActivity extends AppCompatActivity {
    private static String TAG = "MAIN_ACTIVITY";

    private JWPlayer mJWPlayer;
    private AdblockWebView mWebView;
    private boolean isFullscreen = false;

//    private RelativeLayout fullscreenContainer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

//        fullscreenContainer = findViewById(R.id.webViewFullscreenContainer);
//        fullscreenContainer.setVisibility(View.GONE);

        mWebView = findViewById(R.id.webView);
        configureWebView();

        mJWPlayer = new JWPlayer(mWebView);

        mWebView.loadUrl("https://gogo-stream.com/streaming.php?id=MTE3MTU=&title=Bleach+Episode+366");
    }

    public void testBtnClick(View v){
        mJWPlayer.playAsync(() -> {
            Log.d(TAG, "Playing.");
        });
    }

    private void configureWebView(){
        mWebView.getSettings().setJavaScriptEnabled(true);
//        mWebView.getSettings().setUserAgentString("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");

        mWebView.setWebViewClient(new WebViewClient(){
            @Override
            public void onPageFinished(WebView view, String url) {
                super.onPageFinished(view, url);
            }
        });
        mWebView.setWebChromeClient(new WebChromeClient(){
            private View mFullscreenView;
            private CustomViewCallback mCustomCallback;
            @Override
            public boolean onConsoleMessage(ConsoleMessage consoleMessage) {
                super.onConsoleMessage(consoleMessage);

                Log.d(TAG, consoleMessage.message());
                return true;
            }

            @Override
            public void onShowCustomView(View view, CustomViewCallback callback) {
                if(mFullscreenView != null){
                    callback.onCustomViewHidden();
                    return;
                }

                mFullscreenView = view;
                mWebView.setVisibility(View.GONE);
                ViewGroup rootLayout = (ViewGroup)mWebView.getRootView();
                rootLayout.addView(mFullscreenView);

//                fullscreenContainer.setVisibility(View.VISIBLE);
//                fullscreenContainer.addView(mFullscreenView);

                mCustomCallback = callback;
            }

            @Override
            public void onHideCustomView() {
                super.onHideCustomView();
                if(mFullscreenView == null){
                    return;
                }

                ViewGroup rootLayout = (ViewGroup)mWebView.getRootView();
                mWebView.setVisibility(View.VISIBLE);

                mFullscreenView.setVisibility(View.GONE);
//                fullscreenContainer.setVisibility(View.GONE);

//                fullscreenContainer.removeView(mFullscreenView);
                rootLayout.removeView(mFullscreenView);
                mCustomCallback.onCustomViewHidden();

                mFullscreenView = null;
            }
        });
    }
}