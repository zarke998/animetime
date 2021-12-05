﻿using System;
using System.Runtime.InteropServices;

namespace AnimeTime.WPF.Utility.Win32Structs
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
