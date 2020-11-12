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
using AnimeTime.Utilities;

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
        public ICrawlDelayer CrawlDelayer { get; set ; }

        public AnimePlanetRepository(IAnimeInfoExtractor extractor, IAnimeInfoResolver resolver)
        {
            _extractor = extractor;
            _resolver = resolver;

            _extractor.CrawlDelayer = CrawlDelayer;
            _resolver.CrawlDelayer = CrawlDelayer;

            CanFetchByDateAdded = UrlIsAvailable(AnimeListByDateUrl);
            CurrentPage = AnimeListByDateUrl;
        }

        public AnimeDetailedInfo Resolve(AnimeBasicInfo basicInfo)
        {
            _resolver.CrawlDelayer = CrawlDelayer;

            var detailedInfo = _resolver.Resolve(basicInfo);

            Log.TraceEvent(TraceEventType.Verbose, 0, $"\n{detailedInfo}");
            Log.TraceEvent(TraceEventType.Verbose, 0, String.Empty);

            return detailedInfo;
        }
        public IEnumerable<AnimeDetailedInfo> ResolveRange(IEnumerable<AnimeBasicInfo> basicInfos)
        {
            List<Anime> animeList = new List<Anime>();

            //foreach(var animeResolve in animeInfoResolves)
            return null;
        }

        public IEnumerable<AnimeBasicInfo> GetByDate()
        {
            _extractor.CrawlDelayer = CrawlDelayer;
            return _extractor.GetFromPage(CurrentPage);
        }
        public IEnumerable<AnimeBasicInfo> GetAll()
        {
            _extractor.CrawlDelayer = CrawlDelayer;

            var basicInfos = new List<AnimeBasicInfo>();

            var page = AnimeListUrl;
            var endOfFetching = false;

            while (!endOfFetching)
            {
                basicInfos.AddRange(_extractor.GetFromPage(page));

                page = _extractor.NextPage();
                if (page == null)
                    endOfFetching = true;
            }

            return basicInfos;
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