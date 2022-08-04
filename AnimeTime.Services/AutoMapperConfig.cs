using AnimeTime.Services.Profiles;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Configure()
        {
            var mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.ForAllMaps((map, expr) =>
                {
                    expr.ForAllMembers(membConf =>
                    {
                        membConf.Condition(src => src != null);
                    });
                });

                conf.AddProfile<AnimeSourceProfile>();
                conf.AddProfile<AnimeProfile>();

                conf.AddProfile<EpisodeProfile>();

                conf.AddProfile<VideoSourceProfiile>();

                conf.AddProfile<CategoryProfile>();
                conf.AddProfile<GenreProfile>();
                conf.AddProfile<YearSeasonProfile>();

                conf.AddProfile<ImageProfile>();
                conf.AddProfile<ThumbnailProfile>();

                conf.AddProfile<CharacterProfile>();
            });

            return mapperConfiguration;
        }
    }
}
