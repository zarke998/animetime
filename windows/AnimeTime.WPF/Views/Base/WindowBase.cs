using AnimeTime.WPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.Views.Base
{
    public class WindowBase : Window, ICloseable, IMaximizable
    {
        public void Maximize()
        {
            switch(this.WindowState)
            {
                case WindowState.Normal:
                    this.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;
            }
        }
    }
}
