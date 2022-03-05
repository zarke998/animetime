using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Core.Exceptions;
using HtmlAgilityPack;
using System.Threading;
using System.Diagnostics;
using System.Net;
using AnimeTimeDbUpdater.Utilities;
using AnimeTime.Utilities;
using AnimeTime.Utilities.Web;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimePlanetRepository : IAnimeInfoRepository
    {
        private const int ANIME_PLANET_ITEMS_PER_PAGE = 35;

        private IAnimeInfoExtractor _extractor;
        private IAnimeInfoResolver _resolver;
        private ICrawlDelayer _crawlDelayer;
        private string _animeListLastPageByDate;
        private int _lastPageByDateNumber;
        private int _currentPageByDate;

        public string AnimeListUrl { get; private set; } = "https://www.anime-planet.com/anime/all";
        public string AnimeListByDateUrl { get; private set; } = "https://www.anime-planet.com/anime/all?sort=recent&order=desc";

        public bool CanFetchByDateAdded { get; private set; }
        public string CurrentPage { get; private set; }
        public bool LastPageReached { get; private set; }
        public AnimePlanetRepository(IAnimeInfoExtractor extractor,
                                     IAnimeInfoResolver resolver,
                                     ICrawlDelayer crawlDelayer)
        {
            _extractor = extractor;
            _resolver = resolver;

            _crawlDelayer = crawlDelayer;
            _extractor.CrawlDelayer = _crawlDelayer;
            _resolver.CrawlDelayer = _crawlDelayer;

            CanFetchByDateAdded = UrlIsAvailable(AnimeListByDateUrl);
            CurrentPage = AnimeListByDateUrl;

            _lastPageByDateNumber = GetAnimeListLastPageByDate();
            _currentPageByDate = _lastPageByDateNumber;
        }

        public AnimeDetailedInfo Resolve(AnimeBasicInfo basicInfo)
        {
            _resolver.CrawlDelayer = _crawlDelayer;

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

        public IEnumerable<AnimeBasicInfo> GetAll()
        {
            _extractor.CrawlDelayer = _crawlDelayer;

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
        public IEnumerable<AnimeBasicInfo> GetByDate(int page = 0)
        {
            if (_currentPageByDate <= 0) return null;

            if (page > 0)
                _currentPageByDate = page;

            var animes = _extractor.GetFromPage(GetAnimeListByDatePage(_currentPageByDate));
            _currentPageByDate--;

            return animes;
        }

        public int GetAnimeListPageByAnimeOrder(int order)
        {
            return (_lastPageByDateNumber + 1) - (int)Math.Ceiling(order / (ANIME_PLANET_ITEMS_PER_PAGE * 1.0));
        }

        public void ResetFetchByDate()
        {
            _currentPageByDate = _lastPageByDateNumber;
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
        private int GetAnimeListLastPageByDate()
        {
            HtmlDocument doc = null;
            _crawlDelayer.ApplyDelay(() => doc = new HtmlWeb().Load(AnimeListByDateUrl));

            var xPath = "//div[contains(@class,'pagination')]/ul/li[last() - 1]";
            var lastPageNode = doc.DocumentNode.SelectSingleNode(xPath);
            if (lastPageNode == null)
                throw new Core.Exceptions.NodeNotFoundException(xPath, _animeListLastPageByDate);

            return Int32.Parse(lastPageNode.InnerText.Trim());
        }
        private string GetAnimeListByDatePage(int num)
        {
            return UrlUtil.AddQueryParam(AnimeListByDateUrl, "page", num.ToString());
        }

    }
}