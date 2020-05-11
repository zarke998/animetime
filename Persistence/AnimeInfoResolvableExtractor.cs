using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimeInfoResolvableExtractor : IAnimeInfoResolvableExtractor
    {
        private HtmlWeb _web;
        private HtmlDocument _doc;

        public bool IsFinished { get; private set; }
        public string AnimeListUrl { get ; private set ; }
        public string WebsiteUrl { get; private set; }

        public bool IsSessionStarted { get; private set; }
        public string CurrentPage { get; private set; }

        public AnimeInfoResolvableExtractor(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;

            IsFinished = false;
        }
        public void StartExtractSession(string animeListUrl, string websiteUrl)
        {
            AnimeListUrl = animeListUrl;
            WebsiteUrl = websiteUrl;
            IsSessionStarted = true;
        }
        public void EndExtractSession()
        {
            AnimeListUrl = null;
            WebsiteUrl = null;
            IsSessionStarted = false;
        }
        public IEnumerable<AnimeInfoResolvable> GetResolvablesFromPage(string page)
        {
            CurrentPage = page;

            var animeResolves = new List<AnimeInfoResolvable>();

            CrawlStopwatch.ApplyDelay();

            CrawlStopwatch.BeginCrawlTracking();
            _doc = _web.Load(CurrentPage);
            CrawlStopwatch.EndCrawlTracking();

            LogGroup.Log("\n\n\t\t\t\t Getting resolves from page: " + CurrentPage + "\n\n");
            var animeNodes = _doc.DocumentNode.SelectNodes(".//li[contains(@class,'card')]");
            foreach (var node in animeNodes)
            {
                var animeInfoResolve = GetAnimeInfoResolvable(node.OuterHtml);
                animeResolves.Add(animeInfoResolve);

                LogGroup.Log("Fetched: " + animeInfoResolve.Anime.Title);
            }

            return animeResolves;
        }
        public string NextPage()
        {
            var navNode = _doc.DocumentNode.SelectSingleNode("//div[contains(@class,'pagination')]");
            var nextPageLinkNode = navNode.SelectSingleNode(".//li[contains(@class,'next')]/a");

            if (nextPageLinkNode == null)
            {
                IsFinished = true;
                return null;
            }

            var nextPage = AnimeListUrl + nextPageLinkNode.GetAttributeValue("href","");
            return nextPage;
        }

        private AnimeInfoResolvable GetAnimeInfoResolvable(string xmlNode)
        {           
            var animeResolve = new AnimeInfoResolvable();
            var doc = new HtmlDocument();

            doc.LoadHtml(xmlNode);

            var title = doc.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'cardName')]").InnerText;
            animeResolve.Anime.Title = title;

            var detailsUrl = doc.DocumentNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
            animeResolve.AnimeDetailsUrl = WebsiteUrl + detailsUrl;

            var thumbUrl = doc.DocumentNode.SelectSingleNode(".//img").GetAttributeValue("data-src", "");
            animeResolve.AnimeCoverThumbUrl = WebsiteUrl + thumbUrl;

            var createdId = doc.DocumentNode.SelectSingleNode(".//li").GetAttributeValue("data-id", null);
            if (createdId == null)
                animeResolve.Anime.CreatedId = null;
            else
                animeResolve.Anime.CreatedId = Convert.ToInt32(createdId);

            return animeResolve;
        }
    }
}
