using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using AnimeTimeDbUpdater.Utilities;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimePlanetRepository : IAnimeInfoRepository
    {
        private IAnimeInfoExtractor _extractor;
        private IAnimeInfoResolver _resolver;

        public string AnimeListUrl { get; private set; } = "https://www.anime-planet.com/anime/all";
        public string AnimeListByDateUrl { get; private set; } = "https://www.anime-planet.com/anime/all?sort=recent&order=desc";

        public bool CanFetchByDateAdded { get; private set; }
        public string CurrentPage { get; private set; }
        public bool LastPageReached { get; private set; }

        public AnimePlanetRepository(IAnimeInfoExtractor extractor, IAnimeInfoResolver resolver)
        {
            _extractor = extractor;
            _resolver = resolver;
            CanFetchByDateAdded = UrlIsAvailable(AnimeListByDateUrl);
            CurrentPage = AnimeListByDateUrl;
        }

        public void Resolve(AnimeInfo animeInfo)
        {
            _resolver.Resolve(animeInfo);
            var anime = animeInfo.Anime;

            Log.TraceEvent(TraceEventType.Verbose, 0, $"\n{anime}");

            Log.TraceEvent(TraceEventType.Verbose, 0, "Genres: \n");
            foreach (var genre in anime.Genres)
            {
                Log.TraceEvent(TraceEventType.Verbose, 0, $"\t{genre.Name}");
            }
            Log.TraceEvent(TraceEventType.Verbose, 0, String.Empty);
        }
        public void ResolveRange(IEnumerable<AnimeInfo> animeInfos)
        {
            List<Anime> animeList = new List<Anime>();

            //foreach(var animeResolve in animeInfoResolves)

        }

        public IEnumerable<AnimeInfo> GetByDate()
        {
            return _extractor.GetFromPage(CurrentPage);
        }
        public IEnumerable<AnimeInfo> GetAll()
        {
            List<AnimeInfo> animeInfos = new List<AnimeInfo>();

            var page = AnimeListUrl;
            var endOfFetching = false;

            while (!endOfFetching)
            {
                animeInfos.AddRange(_extractor.GetFromPage(page));

                page = _extractor.NextPage();
                if (page == null)
                    endOfFetching = true;
            }

            return animeInfos;
        }

        public string NextPage()
        {
            var nextPage = _extractor.NextPage();

            if (nextPage == null)
                LastPageReached = true;
            else
                CurrentPage = nextPage;

            return CurrentPage;
        }
        public void ResetToFirstPage()
        {
            CurrentPage = AnimeListByDateUrl;
        }

        private bool UrlIsAvailable(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception e) { return false; }
        }
    }
}