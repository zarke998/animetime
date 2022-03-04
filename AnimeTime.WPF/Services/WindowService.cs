using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels;
using AnimeTime.WPF.ViewModels.Base;
using AnimeTime.WPF.Views;
using AnimeTime.WPF.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace AnimeTime.WPF.Services
{
    public class WindowService : IWindowService
    {
        private Dictionary<Type, WindowBase> _windowsInstances = new Dictionary<Type, WindowBase>();

        public void Load<T>(T viewModel) where T : ViewModelBase
        {
            _windowsInstances.TryGetValue(typeof(T), out WindowBase window);

            if (window == null)
            {
                window = CreateWindow<T>();
                _windowsInstances.Add(typeof(T), window);
            }

            window.DataContext = viewModel;
            window.Closed -= Window_Closed;
            window.Closed += Window_Closed;
            window.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var windowType = sender.GetType();

            var key = _windowsInstances.FirstOrDefault(pair => pair.Value.GetType() == windowType).Key;
            if (key != null)
                _windowsInstances.Remove(key);
        }

        private WindowBase CreateWindow<ViewModelType>() where ViewModelType : ViewModelBase
        {
            WindowBase window;
            if (typeof(ViewModelType) == typeof(PlayerWindowViewModel))
                window = new PlayerWindow();
            else
                throw new Exception($"No matching Window found for view model {typeof(ViewModelType).Name}.");
            return window;
        }
    }
}
