using AnimeTime.Core;
using AnimeTime.Persistence;
using AnimeTime.Services;
using AnimeTime.Services.ModelServices;
using AnimeTime.Services.ModelServices.Interfaces;
using AnimeTime.WebsiteProcessors;
using Autofac;
using Autofac.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace AnimeTime.WebAPI
{
    public static class ContainerConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AnimeTimeDbContext>();
            builder.RegisterType<WebsiteProcessorFactory>().As<IWebsiteProcessorFactory>();

            builder.Register(context =>
            {
                var mapperConfig = AutoMapperConfig.Configure();
                var mapper = mapperConfig.CreateMapper();

                return mapper;
            }).As<IMapper>();

            builder.RegisterType<AnimeSourceService>().As<IAnimeSourceService>();


            return builder.Build();
        }
    }
}
