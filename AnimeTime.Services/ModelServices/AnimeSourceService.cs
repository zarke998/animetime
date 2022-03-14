using AnimeTime.Services.DTO;
using AnimeTime.Services.ModelServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core;
using AnimeTime.WebsiteProcessors;
using AnimeTime.WebsiteProcessors.Models;
using AutoMapper;

namespace AnimeTime.Services.ModelServices
{
    public class AnimeSourceService : IAnimeSourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProcessorFactory _websiteProcessorFactory;
        private readonly IMapper _mapper;

        public AnimeSourceService(IUnitOfWork unitOfWork, IWebsiteProcessorFactory websiteProcessorFactory, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._websiteProcessorFactory = websiteProcessorFactory;
            this._mapper = mapper;
        }
        public IEnumerable<AnimeSourceDTO> GetAll(int animeId)
        {
            var animeSources = _unitOfWork.AnimeSources.Find(a => a.AnimeId == animeId);
            if (animeSources.Count() != 0) return _mapper.Map<IEnumerable<AnimeSourceDTO>>(animeSources);

            var anime = _unitOfWork.Animes.GetWithAltTitles(animeId);
            var websites = _unitOfWork.Websites.GetAll();
            var result = new List<Core.Domain.AnimeSource>(); 
            foreach (var website in websites)
            {
                var processor = _websiteProcessorFactory.CreateWebsiteProcessor(website.Name, website.Url, website.QuerySuffix);
                var source = processor.TryFindAnime(new AnimeSearchParams(anime.Title, anime.AltTitles.Select(x => x.Title), anime.ReleaseYear)).Result;

                var sub = _mapper.Map<Core.Domain.AnimeSource>(source.Sub);
                var dub = _mapper.Map<Core.Domain.AnimeSource>(source.Dub);

                sub.AnimeId = anime.Id;
                sub.WebsiteId = website.Id;
                sub.AnimeVersion_Id = Core.Domain.Enums.AnimeVersionIds.Sub;

                dub.AnimeId = anime.Id;
                dub.WebsiteId = website.Id;
                dub.AnimeVersion_Id = Core.Domain.Enums.AnimeVersionIds.Dub;

                result.Add(sub);
                result.Add(dub);
            }
            _unitOfWork.AnimeSources.AddRange(result);
            _unitOfWork.Complete();

            return _mapper.Map<IEnumerable<AnimeSourceDTO>>(result);
        }
    }
}
