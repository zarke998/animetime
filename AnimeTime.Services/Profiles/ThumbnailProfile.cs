using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.Profiles
{
    public class ThumbnailProfile : Profile
    {
        public ThumbnailProfile()
        {
            CreateMap<Thumbnail, ThumbnailDTO>()
                .ForMember(dto => dto.ImageLodLevel, conf => conf.MapFrom(t => t.ImageLodLevel.Name));
        }
    }
}
