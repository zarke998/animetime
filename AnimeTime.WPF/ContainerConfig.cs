using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.WPF.ViewModels;
using AnimeTime.WPF.ViewModels.Pages;
using Autofac;

namespace AnimeTime.WPF
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<HomeViewModel>();

            return builder.Build();
        }
    }
}
