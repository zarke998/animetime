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
using System.Windows.Controls;

namespace AnimeTime.WPF.Views.Base
{
    public class WindowBase : Win32Window, ICloseable, IMaximizable, IMinimizable, INotifyPropertyChanged
    {
        private const int IMAGINARY_BORDER = 7;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isFullscreen;
        public bool IsFullscreen { get => _isFullscreen; set { _isFullscreen = value; OnPropertyChanged(); } }

        #region Template
        protected Border T_WindowBorder { get; set; }
        #endregion

        public WindowBase()
        {
            this.MonitorChanged += WindowBase_MonitorChanged;
            this.Loaded += WindowBase_Loaded;
            this.StateChanged += WindowBase_StateChanged;
        }

        private void InitializeTemplateVariables()
        {
            T_WindowBorder = this.Template.FindName("windowBorder", this) as Border;
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
        private void WindowBase_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                IsFullscreen = true;
            else
                IsFullscreen = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeTemplateVariables();
            //T_WindowBorder.SetCurrentValue(MarginProperty, new Thickness(7));
            T_WindowBorder.Margin = new Thickness(7);
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
            //MaxHeight = SystemInfoUtil.GetScreenWorkAreaHeight(this) + IMAGINARY_BORDER * 2;
        }

        protected virtual void FulllscreenToggle(bool isFullscreen)
        {
            if (isFullscreen)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;

            IsFullscreen = isFullscreen;
        }
        
    }
}
