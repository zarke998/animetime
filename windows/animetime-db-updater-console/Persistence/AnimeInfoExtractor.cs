using AnimeTime.Utilities;
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

            CrawlDelayer.ApplyDelay(() => { _doc = _web.Load(page); });
            LoadedPage = page;

            Log.TraceEvent(TraceEventType.Information, 0, $"\nGetting infos from page: {LoadedPage}\n");

            var animeNodes = _doc.DocumentNode.SelectNodes(".//li[contains(@class,'card')]");
            foreach (var node in animeNodes)
            {
                var info = GetAnimeInfo(node.OuterHtml);
                infos.Add(info);

                Log.TraceEvent(TraceEventType.Information, 0, $"Fetched: {info.Anime.Title}");
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

            return info;
        }
    }
}
