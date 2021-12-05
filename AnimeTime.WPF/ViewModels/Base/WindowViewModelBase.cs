using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels.Base
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand MaximizeCommand { get; set; }
        public DelegateCommand MinimizeCommand { get; set; }

        public WindowViewModelBase()
        {
            CloseCommand = new DelegateCommand(Close);
            MaximizeCommand = new DelegateCommand(Maximize);
            MinimizeCommand = new DelegateCommand(Minimize);
        }

        private void Minimize(object param)
        {
            var minimizable = param as IMinimizable;
            minimizable?.Minimize();
        }

        public void Maximize(object param)
        {
            var maximizable = param as IMaximizable;
            maximizable?.ToggleMaximize();
        }

        public void Close(object param)
        {
            var closeable = param as ICloseable;
            closeable?.Close();
        }
    }
}
