using AnimeTime.Core.Domain;
using AnimeTime.Persistence;
using AnimeTime.WebAPI.Core.WebsiteProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AnimeTime.WebAPI.Controllers
{
    [RoutePrefix("api/animes")]
    public class AnimesController : ApiController
    {
        public string GetAll()
        {
            return "All";
        }
        
        [Route("{id}/episodes-with-sources")]
        public string GetEpisodesWithSources(int id)
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            var episodes = unitOfWork.Episodes.GetWithSources(id);

            //if (episodes.Count() > 0) return episodes;

            var website = new Website()
            {
                Url = "https://gogoanime.so",
                QuerySuffix = "search.html?keyword="
            };

            var anime = unitOfWork.Animes.Get(id);

            var websiteProcessor = new GogoanimeWebsiteProcessor(website);
            return websiteProcessor.GetAnimeUrl(anime.Title);
        }
    }
}
