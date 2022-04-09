using AnimeTime.WPF.Common;
using AnimeTime.WPF.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.Views.Base
{
    public class WindowBase : Win32Window, ICloseable, IMaximizable, IMinimizable, INotifyPropertyChanged
    {
        private const int IMAGINARY_BORDER = 7;

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowBase()
        {
            this.MonitorChanged += WindowBase_MonitorChanged;
            this.Loaded += WindowBase_Loaded;
        }
        #region Events
        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            SetMaximizeHeight();
        }

        private void WindowBase_MonitorChanged(object sender, EventArgs e)
        {
            SetMaximizeHeight();
        }
        #endregion

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

        protected virtual void OnPropertyChanged([CallerMemberName] string callerName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        private void SetMaximizeHeight()
        {
            MaxHeight = SystemInfoUtil.GetScreenWorkAreaHeight(this) + IMAGINARY_BORDER * 2;
        }
    }
}
