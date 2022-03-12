using AnimeTime.Core;
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

        public IEnumerable<AnimeSearchDTO> Search(string searchString)
        {
            return _mapper.Map<IEnumerable<AnimeSearchDTO>>(_unitOfWork.Animes.Search(searchString));
        }
    }
}
