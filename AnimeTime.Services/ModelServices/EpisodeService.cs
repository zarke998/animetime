using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AnimeTime.Services.ModelServices.Interfaces;
using AnimeTime.WebsiteProcessors;
using AnimeTime.WebsiteProcessors.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.ModelServices
{
    public class EpisodeService : IEpisodeService
    {
        private const int EPISODE_REFRESH_INTERVAL = 24;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProcessorFactory _websiteProcessorFactory;
        private readonly IMapper _mapper;
        private readonly IAnimeSourceService _animeSourceService;

        public EpisodeService(IUnitOfWork unitOfWork, 
                              IWebsiteProcessorFactory websiteProcessorFactory, 
                              IMapper mapper,
                              IAnimeSourceService animeSourceService)
        {
            this._unitOfWork = unitOfWork;
            this._websiteProcessorFactory = websiteProcessorFactory;
            this._mapper = mapper;
            this._animeSourceService = animeSourceService;
        }
        public IEnumerable<EpisodeDTO> GetEpisodes(int animeId)
        {
            var animeMetadata = _unitOfWork.AnimeMetadatas.Get(animeId);
            if (animeMetadata != null &&
                animeMetadata.EpisodesLastUpdate.HasValue &&
                (DateTime.UtcNow - animeMetadata.EpisodesLastUpdate.Value).TotalHours < EPISODE_REFRESH_INTERVAL)
            {
                return _mapper.Map<IEnumerable<EpisodeDTO>>(_unitOfWork.Episodes.Find(e => e.AnimeId == animeId));
            }

            var episodes = _unitOfWork.Episodes.Find(e => e.AnimeId == animeId);
            var episodeNums = episodes.Select(ep => ep.EpNum);
            var scrapedEpisodes = ScrapeAllEpisodes(animeId);

            var newEpisodes = scrapedEpisodes.Where(e => !episodeNums.Contains(e.EpNum));

            _unitOfWork.Episodes.AddRange(newEpisodes);
            UpdateEpisodeLastUpdate(animeMetadata, animeId);
            _unitOfWork.Complete();

            return _mapper.Map<IEnumerable<EpisodeDTO>>(scrapedEpisodes);
        }

        private void UpdateEpisodeLastUpdate(AnimeMetadata animeMetadata, int animeId)
        {
            if(animeMetadata == null)
            {
                animeMetadata = new AnimeMetadata() { Id = animeId };
                _unitOfWork.AnimeMetadatas.Add(animeMetadata);
            }

            animeMetadata.EpisodesLastUpdate = DateTime.UtcNow;
        }

        private List<Episode> ScrapeAllEpisodes(int animeId)
        {
            _animeSourceService.GetAll(animeId);

            var anime = _unitOfWork.Animes.GetWithSources(animeId, true);
            var newEpisodes = new List<Episode>();
            foreach (var animeSource in anime.AnimeSources)
            {
                var processor = _websiteProcessorFactory.CreateWebsiteProcessor(animeSource.Website.Name, animeSource.Website.Url, animeSource.Website.QuerySuffix);
                var episodeSources = processor.GetEpisodesAsync(animeSource.Url).Result;
                foreach (var episodeSource in episodeSources)
                {
                    var newEp = newEpisodes.FirstOrDefault(e => e.EpNum == episodeSource.EpisodeNumber);
                    if (newEp == null)
                    {
                        newEp = new Episode() { EpNum = episodeSource.EpisodeNumber, AnimeId = animeId };
                        newEpisodes.Add(newEp);
                    }

                    newEp.Sources.Add(new Core.Domain.EpisodeSource() { Url = episodeSource.Url, AnimeVersionId = animeSource.AnimeVersion_Id, WebsiteId = animeSource.WebsiteId });
                }
            }

            return newEpisodes;
        }
    }
}
