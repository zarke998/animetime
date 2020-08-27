using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimeInfoExtractor : IAnimeInfoExtractor
    {
        private HtmlWeb _web;
        private HtmlDocument _doc;

        public string LoadedPage { get; private set; }

        public AnimeInfoExtractor(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;
        }

        public IEnumerable<AnimeInfo> GetFromPage(string page, string websiteUrl = "")
        {
            var animeResolves = new List<AnimeInfo>();
            
            CrawlDelayer.ApplyDelay();

            CrawlDelayer.BeginCrawlTracking();
            _doc = _web.Load(page);
            LoadedPage = page;
            CrawlDelayer.EndCrawlTracking();

            LogGroup.Log("\n\n\t\t\t\t Getting resolves from page: " + LoadedPage + "\n\n");

            var animeNodes = _doc.DocumentNode.SelectNodes(".//li[contains(@class,'card')]");
            foreach (var node in animeNodes)
            {
                var animeInfoResolve = GetAnimeInfoResolvable(node.OuterHtml, websiteUrl);
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
                return null;

            var nextPage = HttpUtility.HtmlDecode(nextPageLinkNode.GetAttributeValue("href",""));
            return nextPage;
        }
        private AnimeInfo GetAnimeInfoResolvable(string xmlNode, string websiteUrl = "")
        {           
            var animeResolve = new AnimeInfo();
            var doc = new HtmlDocument();

            doc.LoadHtml(xmlNode);

            var title = doc.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'cardName')]").InnerText;
            animeResolve.Anime.Title = title;

            var detailsUrl = doc.DocumentNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
            animeResolve.AnimeDetailsUrl = websiteUrl + detailsUrl;

            var thumbUrl = doc.DocumentNode.SelectSingleNode(".//img").GetAttributeValue("data-src", "");
            animeResolve.AnimeCoverThumbUrl = websiteUrl + thumbUrl;

            var createdId = doc.DocumentNode.SelectSingleNode(".//li").GetAttributeValue("data-id", null);
            if (createdId == null)
                animeResolve.Anime.CreatedId = null;
            else
                animeResolve.Anime.CreatedId = Convert.ToInt32(createdId);

            return animeResolve;
        }
    }
}
