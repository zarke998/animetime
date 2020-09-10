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

        public IEnumerable<AnimeInfo> GetFromPage(string page)
        {
            var infos = new List<AnimeInfo>();
            
            CrawlDelayer.ApplyDelay();

            CrawlDelayer.BeginCrawlTracking();
            _doc = _web.Load(page);
            LoadedPage = page;
            CrawlDelayer.EndCrawlTracking();

            LogGroup.Log("\n\n\t\t\t\t Getting resolves from page: " + LoadedPage + "\n\n");

            var animeNodes = _doc.DocumentNode.SelectNodes(".//li[contains(@class,'card')]");
            foreach (var node in animeNodes)
            {
                var info = GetAnimeInfo(node.OuterHtml);
                infos.Add(info);

                LogGroup.Log("Fetched: " + info.Anime.Title);
            }

            return infos;
        }
        public string NextPage()
        {
            var navNode = _doc.DocumentNode.SelectSingleNode("//div[contains(@class,'pagination')]");
            var nextPageLinkNode = navNode.SelectSingleNode(".//li[contains(@class,'next')]/a");

            if (nextPageLinkNode == null)
                return null;

            var nextPage = HttpUtility.HtmlDecode(nextPageLinkNode.GetAttributeValue("href",""));
            return Constants.WebsiteUrls.AnimePlanet + nextPage;
        }
        private AnimeInfo GetAnimeInfo(string xmlNode)
        {           
            var info = new AnimeInfo();
            var doc = new HtmlDocument();

            doc.LoadHtml(xmlNode);

            var title = doc.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'cardName')]").InnerText;
            info.Anime.Title = title;

            var detailsUrl = doc.DocumentNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
            info.AnimeDetailsUrl = Constants.WebsiteUrls.AnimePlanet + detailsUrl;

            var thumbUrl = doc.DocumentNode.SelectSingleNode(".//img").GetAttributeValue("data-src", "");
            info.AnimeCoverThumbUrl = Constants.WebsiteUrls.AnimePlanet + thumbUrl;

            return info;
        }
    }
}
