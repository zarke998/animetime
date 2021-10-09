using AnimeTime.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace AnimeTime.WPF.Views.Base
{
    public abstract class Win32Window : Window
    {
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MAXIMIZE = 0xF030;

        private IntPtr _activeMonitor;
        public event EventHandler MonitorChanged;

        public Win32Window()
        {
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            _activeMonitor = SystemInfoUtil.MonitorFromWindow(handle, SystemInfoUtil.MONITOR_DEFAULTTONEAREST);

            HwndSource.FromHwnd(handle).AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var monitor = SystemInfoUtil.MonitorFromWindow(hwnd, SystemInfoUtil.MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero && monitor != _activeMonitor)
            {
                _activeMonitor = monitor;
                OnMonitorChanged(new EventArgs());
            }

            return IntPtr.Zero;
        }

        private void OnMonitorChanged(EventArgs eventArgs)
        {
            MonitorChanged?.Invoke(this, eventArgs);
        }
    }
}
