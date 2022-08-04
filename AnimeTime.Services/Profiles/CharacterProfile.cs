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
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterDTO>()
                .ForMember(dto => dto.Role, conf => conf.MapFrom(c => c.Role.RoleName));
        }
    }
}
