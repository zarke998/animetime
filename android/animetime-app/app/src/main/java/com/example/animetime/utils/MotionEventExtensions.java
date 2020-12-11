package com.example.animetime.utils;

import android.os.SystemClock;
import android.view.MotionEvent;

public class MotionEventExtensions {
    public static MotionEvent getTouchDownEvent(float x, float y){
        long startTime = SystemClock.uptimeMillis();
        long endTime = SystemClock.uptimeMillis() + 100;

        MotionEvent event = MotionEvent.obtain(startTime, endTime, MotionEvent.ACTION_DOWN, x, y, 0);

        return event;
    }
    public static MotionEvent getTouchUpEvent(float x, float y){
        long startTime = SystemClock.uptimeMillis();
        long endTime = SystemClock.uptimeMillis() + 100;

        MotionEvent event = MotionEvent.obtain(startTime, endTime, MotionEvent.ACTION_UP, x, y, 0);

        return event;
    }
}
