using AnimeTime.DesktopClient;
using AnimeTime.WPF.ViewModels;
using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            QuickConverter.EquationTokenizer.AddNamespace("System", Assembly.GetAssembly(typeof(object)));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var mainWindowViewModel = scope.Resolve<MainWindowViewModel>();
                var mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };

                mainWindow.Show();
            }

            base.OnStartup(e);
        }
    }
}
