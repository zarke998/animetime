using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AnimeTime.Services.ModelServices.Interfaces;
using AnimeTime.WebsiteProcessors;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.ModelServices
{
    public class VideoSourceService : IVideoSourceService
    {
        private const int VIDEO_SOURCE_REFRESH_INTERVAL = 24;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProcessorFactory _websiteProcessorFactory;
        private readonly IMapper _mapper;

        public VideoSourceService(IUnitOfWork unitOfWork, IWebsiteProcessorFactory websiteProcessorFactory, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._websiteProcessorFactory = websiteProcessorFactory;
            this._mapper = mapper;
        }
        public IEnumerable<VideoSourceDTO> GetVideoSources(int episodeId)
        {
            var episode = _unitOfWork.Episodes.GetWithVideoSources(episodeId);

            var episodeMetadata = _unitOfWork.EpisodeMetadatas.Get(episodeId);
            if(episodeMetadata != null &&
                episodeMetadata.VideoSourcesLastUpdate.HasValue &&
                (DateTime.UtcNow - episodeMetadata.VideoSourcesLastUpdate.Value).TotalHours > VIDEO_SOURCE_REFRESH_INTERVAL)
            {
                
                return _mapper.Map<IEnumerable<VideoSourceDTO>>(episode.Sources.SelectMany(x => x.VideoSources));
            }

            _unitOfWork.EpisodeVideoSources.RemoveRange(episode.Sources.SelectMany(x => x.VideoSources));

            var newVideoSources = new List<EpisodeVideoSource>();
            foreach (var episodeSource in episode.Sources)
            {
                var website = _unitOfWork.Websites.Get(episodeSource.WebsiteId);
                var processor = _websiteProcessorFactory.CreateWebsiteProcessor(website.Name, website.Url, website.QuerySuffix);
                var videoSources = processor.GetVideoSourcesAsync(episodeSource.Url).Result;

                newVideoSources.AddRange(videoSources.Select(videoSource => new EpisodeVideoSource() 
                { 
                    EpisodeSource_Id = episodeSource.Id, 
                    Url = videoSource 
                }));
            }

            _unitOfWork.EpisodeVideoSources.AddRange(newVideoSources);
            UpdateVideoSourcesLastUpdate(episodeMetadata, episodeId);
            _unitOfWork.Complete();

            return _mapper.Map<IEnumerable<VideoSourceDTO>>(newVideoSources);
        }

        private void UpdateVideoSourcesLastUpdate(EpisodeMetadata episodeMetadata, int episodeId)
        {
            if (episodeMetadata == null)
            {
                episodeMetadata = new EpisodeMetadata() { Id = episodeId };
                _unitOfWork.EpisodeMetadatas.Add(episodeMetadata);
            }

            episodeMetadata.VideoSourcesLastUpdate = DateTime.UtcNow;
        }
    }
}
