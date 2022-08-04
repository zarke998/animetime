using AnimeTime.Core;
using AnimeTime.Core.Exceptions;
using AnimeTime.Services.DTO;
using AnimeTime.Services.ModelServices.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.ModelServices
{
    public class AnimeService : IAnimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public AnimeDTO GetAnimeShort(int id)
        {
            var anime = _unitOfWork.Animes.Get(id);
            if (anime == null)
                throw new EntityNotFoundException($"Anime with id {id} not found.");

            return _mapper.Map<AnimeDTO>(anime);
        }

        public AnimeLongDTO GetAnimeLong(int id)
        {
            var anime = _unitOfWork.Animes.GetLongInfo(id);
            if (anime == null)
                throw new EntityNotFoundException($"Anime with id {id} not found.");

            return _mapper.Map<AnimeLongDTO>(anime);
        }

        public IEnumerable<AnimeSearchDTO> Search(string searchString)
        {
            return _mapper.Map<IEnumerable<AnimeSearchDTO>>(_unitOfWork.Animes.Search(searchString));
        }
    }
}
