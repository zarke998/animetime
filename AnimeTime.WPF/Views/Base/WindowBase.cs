using AnimeTime.WPF.Common;
using AnimeTime.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.Views.Base
{
    public class WindowBase : Win32Window, ICloseable, IMaximizable, IMinimizable
    {
        private const int IMAGINARY_BORDER = 7;        

        public WindowBase()
        {
            this.MonitorChanged += WindowBase_MonitorChanged;
            this.Loaded += WindowBase_Loaded;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            SetMaximizeHeight();
        }

        private void WindowBase_MonitorChanged(object sender, EventArgs e)
        {
            SetMaximizeHeight();
        }

        #region Interfaces
        public void ToggleMaximize()
        {
            switch (this.WindowState)
            {
                case WindowState.Normal:
                    SetMaximizeHeight();
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;
            }
        }

        public void Minimize()
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        private void SetMaximizeHeight()
        {
            MaxHeight = SystemInfoUtil.GetScreenWorkAreaHeight(this) + IMAGINARY_BORDER * 2;
        }
    }
}
