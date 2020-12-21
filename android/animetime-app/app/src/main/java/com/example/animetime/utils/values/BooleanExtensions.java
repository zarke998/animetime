package com.example.animetime.utils.values;

public class BooleanExtensions {
    public static Boolean tryGetBoolFromStr(String stringedBool) {
        if (stringedBool == null) return null;

        stringedBool = stringedBool.trim().replace("\"", "");

        if (stringedBool.equals("true")) return true;
        else if (stringedBool.equals("false")) return false;
        else return null;
    }
}
