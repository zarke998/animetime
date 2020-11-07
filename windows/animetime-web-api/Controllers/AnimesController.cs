
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

namespace AnimeTime.WebAPI.Controllers
{
    [RoutePrefix("api/animes")]
    public class AnimesController : ApiController
    {
        private const int _episodeUpdateInterval = 6;

        public string GetAll()
        {
            return "All";
        }

        [Route("{id}/episodes-with-sources")]
        public IHttpActionResult GetEpisodesWithSources(int id)
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            if (!unitOfWork.Animes.Exists(a => a.Id == id))
            {
                return NotFound();
            }

            var animeMetadata = unitOfWork.AnimeMetadatas.Get(id);
            if (animeMetadata != null && animeMetadata.EpisodesLastUpdate != null)
            {
                if ((animeMetadata.EpisodesLastUpdate.Value - DateTime.UtcNow).TotalHours < _episodeUpdateInterval)
                {
                    return Ok(unitOfWork.Episodes.GetWithSources(id));
                }
                else
                {
                    if (UpdateEpisodes(id))
                    {
                        animeMetadata.EpisodesLastUpdate = DateTime.UtcNow;

                        unitOfWork.Complete();
                    }
                    return Ok(unitOfWork.Episodes.GetWithSources(id));
                }
            }
            else
            {
                if (UpdateEpisodes(id))
                {
                    if (animeMetadata == null)
                    {
                        unitOfWork.AnimeMetadatas.Add(new AnimeMetadata() { Id = id, EpisodesLastUpdate = DateTime.UtcNow });

                        try
                        {
                            unitOfWork.Complete();
                        }
                        catch (Exception e)
                        {
                            // Log exception
                        }
                    }
                    else
                    {
                        animeMetadata.EpisodesLastUpdate = DateTime.UtcNow;
                        try
                        {
                            unitOfWork.Complete();
                        }
                        catch (Exception e)
                        {
                            // Log exception
                        }
                    }
                }
                return Ok(unitOfWork.Episodes.GetWithSources(id));
            }
        }

        private bool UpdateEpisodes(int animeId)
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            var anime = unitOfWork.Animes.GetWithSources(animeId, true);

            if (anime.AnimeSources.Count == 0) return false;

            var newEpisodes = new List<Episode>();

            bool atLeastOneSourceUsed = false;
            foreach (var source in anime.AnimeSources)
            {
                if (source.Status_Id == AnimeSourceStatusIds.Conflict || source.Status_Id == AnimeSourceStatusIds.CouldNotResolve) continue;

                var websiteProcessor = WebsiteProcessorFactory.CreateWebsiteProcessor(source.Website.Name, source.Website.Url, source.Website.QuerySuffix);
                var episodes = websiteProcessor.GetEpisodes(source.Url);
                
                // If anime has half episodes (eg. 18.5), this calculation will be incorrect
                var episodeNumDifference = episodes.Count() - anime.Episodes.Count;
                if (episodeNumDifference <= 0) continue;

                foreach (var newEpisode in episodes.Skip(anime.Episodes.Count))
                {
                    var episode = newEpisodes.FirstOrDefault(e => e.EpNum == newEpisode.epNum);
                    if (episode == null)
                    {
                        episode = new Episode() { EpNum = newEpisode.epNum };
                    }

                    episode.Sources.Add(new EpisodeSource() { Url = newEpisode.epUrl, WebsiteId = source.WebsiteId });
                    newEpisodes.Add(episode);
                }
                atLeastOneSourceUsed = true;
            }
            newEpisodes.ForEach(e => anime.Episodes.Add(e));
            try
            {
                unitOfWork.Complete();
            }
            catch (EntityInsertException insertException)
            {
                // Log exception

                atLeastOneSourceUsed = false;
            }

            return atLeastOneSourceUsed;
        }
    }
}
