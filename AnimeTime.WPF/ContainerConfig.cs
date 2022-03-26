using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.WPF.Services;
using AnimeTime.WPF.Services.Interfaces;
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

            builder.RegisterType<WindowService>().As<IWindowService>().SingleInstance();

            builder.RegisterType<ViewModelLocator>().As<IViewModelLocator>().SingleInstance()
                                                                            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            builder.RegisterType<MainWindowViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerWindowViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<DetailsViewModel>().InstancePerLifetimeScope();

            builder.RegisterType<HomeViewModel>().InstancePerLifetimeScope();

            // Api services
            builder.RegisterType<SearchService>().As<ISearchService>();
            builder.RegisterType<AnimeService>().As<IAnimeService>();
            builder.RegisterType<EpisodeService>().As<IEpisodeService>();

            return builder.Build();
        }
    }
}
