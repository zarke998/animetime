using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Comparers;
using AnimeTime.Core.Exceptions;
using AnimeTime.WebsiteProcessors;

namespace AnimeTime.WebAPI.Controllers
{
    [RoutePrefix("api/episodes")]
    public class EpisodeController : ApiController
    {
        private const int _episodeVideoSourcesUpdateIntervalHours = 8;
        
        [Route("{id}/video-sources")]
        public async Task<IHttpActionResult> GetEpisodeSourcesWithVideoSources(int epId)
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            var episode = unitOfWork.Episodes.Get(epId, true);
            if (episode == null) return NotFound();

            if(episode.Metadata == null || episode.Metadata.VideoSourcesLastUpdate == null || (DateTime.UtcNow - episode.Metadata.VideoSourcesLastUpdate.Value).TotalHours > _episodeVideoSourcesUpdateIntervalHours)
            {
                await UpdateEpisodeVideoSources(epId);

                UpdateVideoSourcesLastUpdate(episode, unitOfWork);

                return Ok(unitOfWork.EpisodeSources.GetByEpisode(epId, true, true));
            }
            else
            {
                return Ok(unitOfWork.EpisodeSources.GetByEpisode(epId, true, true));
            }

        }

        private void UpdateVideoSourcesLastUpdate(Episode episode, Core.IUnitOfWork unitOfWork)
        {
            if(episode.Metadata == null)
            {
                episode.Metadata = new EpisodeMetadata();
            }
            episode.Metadata.VideoSourcesLastUpdate = DateTime.UtcNow;

            unitOfWork.Complete();
        }

        private async Task UpdateEpisodeVideoSources(int epId)
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            var episodeSourcesWithVideoSources = unitOfWork.EpisodeSources.GetByEpisode(epId, true, true);

            foreach(var source in episodeSourcesWithVideoSources)
            {
                var websiteProcessor = WebsiteProcessorFactory.CreateWebsiteProcessor(source.Website.Name, source.Website.Url, source.Website.QuerySuffix);
                if (websiteProcessor == null)
                {
                    // Log no website processor

                    continue;
                }

                var fetchedVideoSources = (await websiteProcessor.GetVideoSourcesForEpisodeAsync(source.Url)).Select(s => new EpisodeVideoSource() { Url = s });

                var outdatedSources = source.VideoSources.Except(fetchedVideoSources, new EpisodeVideoSourceComparer());
                var newSources = fetchedVideoSources.Except(source.VideoSources, new EpisodeVideoSourceComparer());

                unitOfWork.EpisodeVideoSources.RemoveRange(outdatedSources);
                AddNewVideoSources(unitOfWork, newSources, epId);

                try
                {
                    unitOfWork.Complete();
                }
                catch(EntityInsertException ex)
                {
                    // Log insert exception

                    return;
                }
            }
        }
        private void AddNewVideoSources(Core.IUnitOfWork unitOfWork, IEnumerable<EpisodeVideoSource> newSources, int epId)
        {
            foreach (var newSource in newSources)
            {
                newSource.EpisodeSource_Id = epId;
            }
            unitOfWork.EpisodeVideoSources.AddRange(newSources);
        }
    }
}
