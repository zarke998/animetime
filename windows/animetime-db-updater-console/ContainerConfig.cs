﻿using System;
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
using AnimeTime.Utilities.Loggers;
using AnimeTime.Utilities.Imaging;
using AnimeTime.Utilities.Core.Imaging;

namespace AnimeTimeDbUpdater
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainApplication>().As<IApplication>();
            builder.RegisterType<AnimePlanetRepository>().As<IAnimeInfoRepository>();
            builder.RegisterType<AnimePlanetCharacterRepository>().As<ICharacterInfoRepository>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AnimeInfoExtractor>().As<IAnimeInfoExtractor>();
            builder.RegisterType<AnimeInfoResolver>().As<IAnimeInfoResolver>();
            builder.RegisterType<HtmlWeb>();
            builder.RegisterType<HtmlDocument>();

            builder.RegisterType<FileLogger>();
            builder.RegisterType<ConsoleLogger>();

            builder.RegisterType<ImageDownloader>().As<IImageDownloader>();
            builder.RegisterType<ThumbnailGenerator>().As<IThumbnailGenerator>();

            builder.RegisterType<ImageResizer>().As<IImageResizer>();
            builder.RegisterType<JpegCompressor>().As<IJpegCompressor>();

            return builder.Build();
        }
    }
}
