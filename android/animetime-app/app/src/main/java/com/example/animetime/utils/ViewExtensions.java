package com.example.animetime.utils;

import android.view.View;

public class ViewExtensions {
    /**
     * Simulate touch on a view
     * @param view View to simulate click on
     * @param x X coord relative to view
     * @param y Y coord relative to view
     */
    public static void simulateTouch(View view, float x, float y){
        view.dispatchTouchEvent(MotionEventExtensions.getTouchDownEvent(x,y));
        view.dispatchTouchEvent(MotionEventExtensions.getTouchUpEvent(x,y));
    }

    /**
     * Simulates touch on the center of view
     * @param view
     */
    public static void simulateTouchOnCenter(View view){
        int centerX = view.getWidth() / 2;
        int centerY = view.getHeight() / 2;

        view.dispatchTouchEvent(MotionEventExtensions.getTouchDownEvent(centerX, centerY));
        view.dispatchTouchEvent(MotionEventExtensions.getTouchUpEvent(centerX, centerY));
    }
}