using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
using HtmlAgilityPack;
using AnimeTimeDbUpdater.Utilities;
using AnimeTime.Persistence;
using AnimeTime.Core;

namespace AnimeTimeDbUpdater
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainApplication>().As<IApplication>();
            builder.RegisterType<AnimePlanetRepository>().As<IAnimeInfoRepository>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AnimeInfoExtractor>().As<IAnimeInfoExtractor>();
            builder.RegisterType<AnimeInfoResolver>().As<IAnimeInfoResolver>();
            builder.RegisterType<HtmlWeb>();
            builder.RegisterType<HtmlDocument>();

            builder.RegisterType<FileLogger>();
            builder.RegisterType<ConsoleLogger>();


            return builder.Build();
        }
    }
}
