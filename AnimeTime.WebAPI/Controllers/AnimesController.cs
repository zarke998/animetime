﻿
using AnimeTime.Core.Domain;
using AnimeTime.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Core.Exceptions;
using AnimeTime.WebsiteProcessors;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Services.ModelServices.Interfaces;

namespace AnimeTime.WebAPI.Controllers
{
    [RoutePrefix("api/animes")]
    public class AnimesController : ApiController
    {
        private const int _episodeUpdateInterval = 6;

        private readonly IAnimeService _animeService;
        private readonly IAnimeSourceService _animeSourceService;
        private readonly IEpisodeService _episodeService;

        public AnimesController(IAnimeService animeService, IAnimeSourceService animeSourceService, IEpisodeService episodeService)
        {
            this._animeService = animeService;
            this._animeSourceService = animeSourceService;
            this._episodeService = episodeService;
        }

        public string GetAll()
        {
            return "All";
        }

        //[Route("{id}/episodes-with-sources")]
        //public async Task<IHttpActionResult> GetEpisodesWithSources(int id)
        //{
        //    var unitOfWork = ClassFactory.CreateUnitOfWork();

        //    if (!unitOfWork.Animes.Exists(a => a.Id == id))
        //    {
        //        return NotFound();
        //    }

        //    var animeMetadata = unitOfWork.AnimeMetadatas.Get(id);
        //    if (animeMetadata != null && animeMetadata.EpisodesLastUpdate != null)
        //    {
        //        if ((DateTime.UtcNow - animeMetadata.EpisodesLastUpdate.Value).TotalHours < _episodeUpdateInterval)
        //        {
        //            return Ok(unitOfWork.Episodes.GetWithSources(id));
        //        }
        //        else
        //        {
        //            await AddNewEpisodes(id);

        //            animeMetadata.EpisodesLastUpdate = DateTime.UtcNow;
        //            unitOfWork.Complete();

        //            return Ok(unitOfWork.Episodes.GetWithSources(id));
        //        }
        //    }
        //    else
        //    {
        //        await AddNewEpisodes(id);
        //        if (animeMetadata == null)
        //        {
        //            unitOfWork.AnimeMetadatas.Add(new AnimeMetadata() { Id = id, EpisodesLastUpdate = DateTime.UtcNow });

        //            try
        //            {
        //                unitOfWork.Complete();
        //            }
        //            catch (Exception e)
        //            {
        //                // Log exception
        //            }
        //        }
        //        else
        //        {
        //            animeMetadata.EpisodesLastUpdate = DateTime.UtcNow;
        //            try
        //            {
        //                unitOfWork.Complete();
        //            }
        //            catch (Exception e)
        //            {
        //                // Log exception
        //            }
        //        }
        //        return Ok(unitOfWork.Episodes.GetWithSources(id));
        //    }
        //}

        //private async Task<bool> AddNewEpisodes(int animeId)
        //{
        //    var unitOfWork = ClassFactory.CreateUnitOfWork();
        //    var anime = unitOfWork.Animes.GetWithSources(animeId, true);

        //    if (anime.AnimeSources.Count == 0) return false;

        //    var newEpisodes = new List<Episode>();

        //    bool atLeastOneSourceUsed = false;
        //    foreach (var source in anime.AnimeSources)
        //    {
        //        if (source.Status_Id == AnimeSourceStatusIds.Conflict || source.Status_Id == AnimeSourceStatusIds.CouldNotResolve) continue;

        //        var websiteProcessor = WebsiteProcessorFactory.CreateWebsiteProcessor(source.Website.Name, source.Website.Url, source.Website.QuerySuffix);
        //        var episodes = await websiteProcessor.GetAnimeEpisodesAsync(source.Url);

        //        var differentEpisodes = GetDifferentEpisodes(episodes);

        //        if (differentEpisodes.Count() > 0)
        //        {
        //            foreach (var newEpisode in differentEpisodes)
        //            {
        //                var episode = newEpisodes.FirstOrDefault(e => e.EpNum == newEpisode.epNum);
        //                if (episode == null)
        //                {
        //                    episode = new Episode() { EpNum = newEpisode.epNum };
        //                }

        //                episode.Sources.Add(new EpisodeSource() { Url = newEpisode.epUrl, WebsiteId = source.WebsiteId, AnimeVersionId = source.AnimeVersion_Id });
        //                newEpisodes.Add(episode);
        //            }
        //            atLeastOneSourceUsed = true;
        //        }
        //    }
        //    newEpisodes.ForEach(e => anime.Episodes.Add(e));
        //    try
        //    {
        //        unitOfWork.Complete();
        //    }
        //    catch (EntityInsertException insertException)
        //    {
        //        // Log exception

        //        atLeastOneSourceUsed = false;
        //    }
        //    return atLeastOneSourceUsed;

        //    IEnumerable<(float epNum, string epUrl)> GetDifferentEpisodes(IEnumerable<(float epNum, string epUrl)> episodes)
        //    {
        //        var differentEpisodes = new List<(float epNum, string epUrl)>();

        //        if (anime.Episodes.Count == 0 || episodes.Count() == 0) return episodes;

        //        var animeEpisodeNumbers = anime.Episodes.Select(e => e.EpNum);

        //        foreach (var episode in episodes)
        //        {
        //            if (!animeEpisodeNumbers.Contains(episode.epNum))
        //            {
        //                differentEpisodes.Add(episode);
        //            }
        //        }

        //        return differentEpisodes;
        //    }
        //}

        [Route("{id}/short-info")]
        public IHttpActionResult GetShortInfo(int id)
        {
            return Ok(_animeService.GetAnimeShort(id));
        }

        [Route("{id}/long-info")]
        public IHttpActionResult GetLongInfo(int id)
        {
            return Ok(_animeService.GetAnimeLong(id));
        }

        [Route("{id}/anime-sources")]
        public IHttpActionResult GetAnimeSources(int id)
        {
            return Ok(_animeSourceService.GetAll(id));
        }

        [Route("{id}/episodes")]
        public IHttpActionResult GetEpisodes(int id)
        {
            return Ok(_episodeService.GetEpisodes(id));
        }
    }
}
