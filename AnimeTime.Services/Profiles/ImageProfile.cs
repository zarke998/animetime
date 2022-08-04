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
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDTO>()
                .ForMember(dto => dto.ImageType, conf => conf.MapFrom(i => i.ImageType.TypeName));
        }
    }
}
