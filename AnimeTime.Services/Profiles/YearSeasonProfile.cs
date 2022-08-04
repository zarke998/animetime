using AnimeTime.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.Profiles
{
    public class YearSeasonProfile : Profile
    {
        public YearSeasonProfile()
        {
            CreateMap<YearSeason, string>().ConvertUsing(y => y != null ? y.Name : null);
        }
    }
}
