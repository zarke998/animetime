package com.example.animetime;

import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.ConsoleMessage;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;
import android.widget.RelativeLayout;


import com.example.animetime.utils.htmlembedplayers.HtmlEmbedPlayerBase;
import com.example.animetime.utils.htmlembedplayers.JWPlayer;
import com.example.animetime.utils.htmlembedplayers.MixdropPlayer;
import com.example.animetime.utils.htmlembedplayers.StreamtapePlayer;
import com.example.animetime.utils.htmlembedplayers.VideoJsPlayer;
import com.example.animetime.widgets.AnimeTimeWebView;

import org.adblockplus.libadblockplus.android.webview.AdblockWebView;

public class MainActivity extends AppCompatActivity {
    private static String TAG = "MAIN_ACTIVITY";

    private JWPlayer mJWPlayer;
    private MixdropPlayer mMixdropPlayer;
    private StreamtapePlayer mStreamtapePlayer;
    private VideoJsPlayer mMp4UploadPlayer;

    private AnimeTimeWebView mWebView;
    private Button mTestBtn;

    private int mTestCaseIndex = -1;
    private int mRepeatTestIndex;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mWebView = findViewById(R.id.webView);
        mTestBtn = findViewById(R.id.testBtn);

        mJWPlayer = new JWPlayer(mWebView);
        mMixdropPlayer = new MixdropPlayer(mWebView);
        mStreamtapePlayer = new StreamtapePlayer(mWebView);
        mMp4UploadPlayer = new VideoJsPlayer(mWebView);

//        mWebView.loadUrl("https://gogo-stream.com/streaming.php?id=MTE3MTU=&title=Bleach+Episode+366");
//        mWebView.loadUrl("https://mixdrop.co/e/dqkj98rqbez9mj");
//        mWebView.loadUrl("https://streamta.pe/e/bQbAopJKlBsPM78/bleach-episode-366.mp4");
        mWebView.loadUrl("https://www.mp4upload.com/embed-hzl1fw4wkedt.html");
    }


    @SuppressLint("LogNotTimber")
    public void testBtnClick(View v){
        HtmlEmbedPlayerBase player = mMp4UploadPlayer;

        switch (mTestCaseIndex) {
            case -1:
                Log.d(TAG, "Setup test.");
                player.setupAsync(() -> {
                    Log.d(TAG, "Setup done.");
                });
                break;
            case 0:
                Log.d(TAG, "Play test 1.");
                player.playAsync(() -> {
                    Log.d(TAG, "Playing.");
                });
                break;
            case 1:
                Log.d(TAG, "Pause test.");
                player.pauseAsync(() -> {
                    Log.d(TAG, "Paused.");
                });
                break;
            case 2:
                Log.d(TAG, "Play test 2.");
                player.playAsync(() -> {
                    Log.d(TAG, "Playing.");
                });
                break;
            case 3:
                Log.d(TAG, "Seek test.");
                player.seekAsync(900, () -> {
                    Log.d(TAG, "Seeked.");
                });
                break;
            case 4:
                Log.d(TAG, "Duration test.");
                player.getVideoDurationAsync(duration -> {
                    Log.d(TAG, "Video duration:" + duration);
                });
                break;
            case 5:
                Log.d(TAG, "Position test");
                player.getVideoPositionAsync(position -> {
                    Log.d(TAG, "Position: " + position);
                });
                break;
            case 6:
                Log.d(TAG, "Get volume test.");
                player.getVolumeAsync(volume -> {
                    Log.d(TAG, "Volume: " + volume);
                });
                break;
            case 7:
                Log.d(TAG, "Set volume test.");
                player.setVolumeAsync(50, () -> {
                    Log.d(TAG, "Volume set.");
                });
                break;
            case 8:
                Log.d(TAG, "Fullscreen test.");
                if (player.isFullscreenProtected()) {
                    Log.d(TAG, "Fullscreen protected.");
                } else {
                    player.setFullscreenAsync(true, () -> {
                        Log.d(TAG, "Fullscreened.");
                    });
                }
                break;
            case 9:
                Log.d(TAG, "Hide controls test.");
                player.hidePlayerControlsAsync(() -> {
                    Log.d(TAG, "Controls hidden.");
                });
                break;
            default:
                mTestCaseIndex = 0;
                Log.d(TAG, "Tests finished.");
        }
        mRepeatTestIndex = mTestCaseIndex;

        mTestCaseIndex++;
    }

    public void repeatTestBtnClick(View v){
        mTestCaseIndex = mRepeatTestIndex;
        mTestBtn.performClick();
    }
}