using AnimeTime.WPF.Utility.Win32Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace AnimeTime.WPF.Utility
{
    public static class SystemInfoUtil
    {
        public const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        public static int GetScreenWorkAreaHeight(Window window)
        {
            IntPtr handle = (new WindowInteropHelper(window)).Handle;            
            IntPtr monitor = MonitorFromWindow(handle, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
                GetMonitorInfo(monitor, ref monitorInfo);

                return monitorInfo.rcWork.Bottom - monitorInfo.rcWork.Top;
            }

            return 0;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
    }
}
