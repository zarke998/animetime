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
    public class AnimeProfile : Profile
    {
        public AnimeProfile()
        {
            CreateMap<Anime, AnimeSearchDTO>()
                .ForMember(dto => dto.CoverImageUrl, conf =>
                {
                    conf.MapFrom((a, dto) =>
                    {
                        var coverImage = a.Images.FirstOrDefault(i => i.Image == null ? false : (i.Image.ImageType_Id == Core.Domain.Enums.ImageTypeId.Cover));
                        return coverImage?.Image?.Url;
                    });
                });
            CreateMap<Anime, AnimeDTO>();
        }
    }
}
