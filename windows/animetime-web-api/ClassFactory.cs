using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AnimeTime.WebAPI.DTOs.EpisodeSource;
using AnimeTime.WebAPI.DTOs.EpisodeVideoSource;
using AnimeTime.WebAPI.DTOs.Website;

namespace AnimeTime.WebAPI
{
    public static class ClassFactory
    {
        public static IUnitOfWork CreateUnitOfWork() => new UnitOfWork(new AnimeTimeDbContext());
        public static MapperConfiguration AutomapperConfiguration { get; private set; }
        static ClassFactory()
        {
            AutomapperConfiguration = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EpisodeSource, EpisodeSourceDtoNormal>();
                    cfg.CreateMap<EpisodeVideoSource, EpisodeVideoSourceDtoShort>();
                    cfg.CreateMap<Website, WebsiteDtoShort>();
                }
            );
        }
    }
}