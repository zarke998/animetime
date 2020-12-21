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
import com.example.animetime.utils.htmlembedplayers.MixdropPlayer;
import com.example.animetime.utils.htmlembedplayers.StreamtapePlayer;
import com.example.animetime.widgets.AnimeTimeWebView;

import org.adblockplus.libadblockplus.android.webview.AdblockWebView;

public class MainActivity extends AppCompatActivity {
    private static String TAG = "MAIN_ACTIVITY";

    private JWPlayer mJWPlayer;
    private MixdropPlayer mMixdropPlayer;
    private StreamtapePlayer mStreamtapePlayer;

    private AnimeTimeWebView mWebView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mWebView = findViewById(R.id.webView);

        mJWPlayer = new JWPlayer(mWebView);
        mMixdropPlayer = new MixdropPlayer(mWebView);
        mStreamtapePlayer = new StreamtapePlayer(mWebView);

//        mWebView.loadUrl("https://gogo-stream.com/streaming.php?id=MTE3MTU=&title=Bleach+Episode+366");
//        mWebView.loadUrl("https://mixdrop.co/e/dqkj98rqbez9mj");
        mWebView.loadUrl("https://streamta.pe/e/bQbAopJKlBsPM78/bleach-episode-366.mp4");
    }

    public void testBtnClick(View v){
        mStreamtapePlayer.playAsync(() -> {
            Log.d(TAG, "Playing.");
        });
    }}