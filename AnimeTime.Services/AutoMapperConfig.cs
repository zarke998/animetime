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
                conf.AddProfile<AnimeSourceProfile>();
                conf.AddProfile<AnimeProfile>();
            });

            return mapperConfiguration;
        }
    }
}
