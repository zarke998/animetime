using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AnimeTime.WebsiteProcessors.Models;
using AutoMapper;

namespace AnimeTime.Services.Profiles
{
    public class AnimeSourceProfile : Profile
    {
        public AnimeSourceProfile()
        {
            CreateMap<WebsiteProcessors.Models.AnimeSource, Core.Domain.AnimeSource>()
                .ForMember(coreSource => coreSource.Status_Id, conf => conf.MapFrom(procSource => procSource.Status));

            CreateMap<Core.Domain.AnimeSource, AnimeSourceDTO>();
        }
    }
}
