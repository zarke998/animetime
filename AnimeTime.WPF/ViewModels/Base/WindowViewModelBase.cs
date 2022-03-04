using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Common;
using AnimeTime.WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels.Base
{
    public abstract class WindowViewModelBase : CommonViewModelBase
    {
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand MaximizeCommand { get; set; }
        public DelegateCommand MinimizeCommand { get; set; }

        public WindowViewModelBase(IWindowService windowService, IViewModelLocator viewModelLocator) : base(windowService, viewModelLocator)
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
            var shutdownable = param as ICanShutdown;

            if (closeable == null && shutdownable == null) return;

            if (shutdownable != null)
                shutdownable.Shutdown();
            else
                closeable.Close();
        }
    }
}
