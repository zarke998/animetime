package com.example.animetime;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.webkit.ConsoleMessage;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;


import com.example.animetime.utils.htmlembedplayers.JWPlayer;

import org.adblockplus.libadblockplus.android.webview.AdblockWebView;

public class MainActivity extends AppCompatActivity {
    private static String TAG = "MAIN_ACTIVITY";

    private JWPlayer mJWPlayer;
    private AdblockWebView mWebView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mWebView = findViewById(R.id.webView);
        configureWebView();

        mJWPlayer = new JWPlayer(mWebView);

        mWebView.loadUrl("https://gogo-stream.com/streaming.php?id=MTE3MTU=&title=Bleach+Episode+366");
    }

    public void testBtnClick(View v){
        mJWPlayer.seekAsync(900, () -> {
            Log.d(TAG, "Seek finished.");
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
            @Override
            public boolean onConsoleMessage(ConsoleMessage consoleMessage) {
                super.onConsoleMessage(consoleMessage);

                Log.d(TAG, consoleMessage.message());
                return true;
            }
        });
    }
}