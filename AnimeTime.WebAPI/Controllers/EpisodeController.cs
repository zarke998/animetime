using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Comparers;
using AnimeTime.Core.Exceptions;
using AnimeTime.WebAPI.DTOs.EpisodeSource;
using AnimeTime.WebsiteProcessors;

namespace AnimeTime.WebAPI.Controllers
{
    [RoutePrefix("api/episodes")]
    public class EpisodeController : ApiController
    {
        private const int _videoSourcesUpdateIntervalHours = 8;

        //[Route("{id}/video-sources")]
        //public async Task<IHttpActionResult> GetSourcesWithVideoSources(int id)
        //{
        //    var unitOfWork = ClassFactory.CreateUnitOfWork();

        //    var episode = unitOfWork.Episodes.Get(id, true);
        //    if (episode == null) return NotFound();

        //    if (episode.Metadata == null || episode.Metadata.VideoSourcesLastUpdate == null || (DateTime.UtcNow - episode.Metadata.VideoSourcesLastUpdate.Value).TotalHours > _videoSourcesUpdateIntervalHours)
        //    {
        //        await UpdateVideoSources(id);
        //        UpdateVideoSourcesLastUpdate(episode, unitOfWork);

        //        return Ok(GetResult());
        //    }
        //    else
        //    {
        //        return Ok(GetResult());
        //    }

        //    IEnumerable<EpisodeSourceDtoNormal> GetResult()
        //    {
        //        var episodeSources = unitOfWork.EpisodeSources.GetByEpisode(id, true, true);

        //        var mapper = new AutoMapper.Mapper(ClassFactory.AutomapperConfiguration);
        //        var episodeSourcesWithVideoSources = mapper.Map<IEnumerable<EpisodeSource>, IEnumerable<EpisodeSourceDtoNormal>>(episodeSources);
        //        return episodeSourcesWithVideoSources;
        //    }
        //}        

        //private void UpdateVideoSourcesLastUpdate(Episode episode, Core.IUnitOfWork unitOfWork)
        //{
        //    if(episode.Metadata == null)
        //    {
        //        episode.Metadata = new EpisodeMetadata();
        //    }
        //    episode.Metadata.VideoSourcesLastUpdate = DateTime.UtcNow;

        //    unitOfWork.Complete();
        //}

        //private async Task UpdateVideoSources(int epId)
        //{
        //    var unitOfWork = ClassFactory.CreateUnitOfWork();

        //    var episodeSourcesWithVideoSources = unitOfWork.EpisodeSources.GetByEpisode(epId, true, true);

        //    foreach(var episodeSource in episodeSourcesWithVideoSources)
        //    {
        //        var websiteProcessor = WebsiteProcessorFactory.CreateWebsiteProcessor(episodeSource.Website.Name, episodeSource.Website.Url, episodeSource.Website.QuerySuffix);
        //        if (websiteProcessor == null)
        //        {
        //            // Log no website processor

        //            continue;
        //        }

        //        var fetchedVideoSources = (await websiteProcessor.GetVideoSourcesForEpisodeAsync(episodeSource.Url)).Select(s => new EpisodeVideoSource() { Url = s }).ToList();

        //        var outdatedSources = episodeSource.VideoSources.Except(fetchedVideoSources, new EpisodeVideoSourceComparer());
        //        var newSources = fetchedVideoSources.Except(episodeSource.VideoSources, new EpisodeVideoSourceComparer());

        //        unitOfWork.EpisodeVideoSources.RemoveRange(outdatedSources);
        //        AddNewVideoSources(unitOfWork, newSources, episodeSource.Id);

        //        try
        //        {
        //            unitOfWork.Complete();
        //        }
        //        catch(EntityInsertException ex)
        //        {
        //            // Log insert exception

        //            return;
        //        }
        //    }
        //}
        //private void AddNewVideoSources(Core.IUnitOfWork unitOfWork, IEnumerable<EpisodeVideoSource> newSources, int epSourceId)
        //{
        //    foreach (var newSource in newSources)
        //    {
        //        newSource.EpisodeSource_Id = epSourceId;
        //    }
        //    unitOfWork.EpisodeVideoSources.AddRange(newSources);
        //}


    }
}
