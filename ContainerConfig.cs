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

            builder.RegisterType<TestApplication>().As<IApplication>();
            builder.RegisterType<AnimePlanetRepositoryFetcher>().As<IAnimeRepositoryFetcher>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AnimeInfoResolvableExtractor>().As<IAnimeInfoResolvableExtractor>();
            builder.RegisterType<HtmlWeb>();
            builder.RegisterType<HtmlDocument>();

            builder.RegisterType<FileLogger>();
            builder.RegisterType<ConsoleLogger>();

            builder.RegisterType<AnimeInfoResolver>().As<IAnimeInfoResolver>();

            return builder.Build();
        }
    }
}
