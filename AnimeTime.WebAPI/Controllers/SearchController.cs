using AnimeTime.Services.ModelServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AnimeTime.WebAPI.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IAnimeService _animeService;

        public SearchController(IAnimeService animeService)
        {
            this._animeService = animeService;
        }

        // GET: api/Search
        public IHttpActionResult Get([FromUri] string animeName)
        {
            return Ok(_animeService.Search(animeName));
        }
    }
}
