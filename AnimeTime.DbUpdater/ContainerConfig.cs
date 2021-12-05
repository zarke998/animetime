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
using AnimeTime.Utilities.Imaging;
using AnimeTime.Utilities.Core.Imaging;
using AnimeTime.Utilities;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AnimeTimeDbUpdater
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                var configBuilder = new ConfigurationBuilder();

                var config = configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: false)
                            .Build();

                var appSettings = config.Get<AppSettings>();
                return appSettings;
            }).SingleInstance();

            builder.RegisterType<MainApplication>().As<IApplication>();
            builder.RegisterType<AnimePlanetRepository>().As<IAnimeInfoRepository>();
            builder.RegisterType<AnimePlanetCharacterRepository>().As<ICharacterInfoRepository>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AnimeInfoExtractor>().As<IAnimeInfoExtractor>();
            builder.RegisterType<AnimeInfoResolver>().As<IAnimeInfoResolver>();
            builder.RegisterType<HtmlWeb>();
            builder.RegisterType<HtmlDocument>();

            builder.RegisterType<ImageDownloader>().As<IImageDownloader>();
            builder.RegisterType<ThumbnailGenerator>().As<IThumbnailGenerator>();

            builder.RegisterType<ImageResizer>().As<IImageResizer>();
            builder.RegisterType<JpegCompressor>().As<IJpegCompressor>();

            builder.RegisterType<CrawlDelayer>().As<ICrawlDelayer>();
            builder.Register(c =>
            {
                var appSettings = c.Resolve<AppSettings>();
                return new AzureImageStorage(appSettings.AzureBlobStorageConnectionString);
            }).As<IImageStorage>();

            return builder.Build();
        }
    }
}
