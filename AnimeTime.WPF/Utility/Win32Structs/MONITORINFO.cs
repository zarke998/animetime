﻿using System.Runtime.InteropServices;

namespace AnimeTime.WPF.Utility.Win32Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
}
