using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimeInfoResolveExtractor : IAnimeInfoResolveExtractor
    {
        private HtmlWeb _web;
        private HtmlDocument _doc;

        public bool IsFinished { get; private set; }
        public string CurrentPage { get; private set; }
        public string WebsiteUrl { get ; set ; }
        public string AnimeListUrl { get ; set ; }


        public AnimeInfoResolveExtractor(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;
        }

        public void Initialize(string websiteUrl, string animeListUrl)
        {
            CurrentPage = animeListUrl;
            WebsiteUrl = websiteUrl;
            AnimeListUrl = animeListUrl;
        }
        public IEnumerable<AnimeInfoResolve> GetAnimeInfoResolvesFromPage()
        {
            if (WebsiteUrl == null || AnimeListUrl == null)
                throw new NullReferenceException("Extractor not initialized. Use method Initialize() before any work with the extractor.");

            var animeResolves = new List<AnimeInfoResolve>();

            _doc = _web.Load(CurrentPage);
            LogGroup.Log("\t\t\t\t Getting resolves from page: " + CurrentPage + "\n\n");

            var animeNodes = _doc.DocumentNode.SelectNodes(".//li[contains(@class,'card')]");

            foreach (var node in animeNodes)
            {
                var animeInfoResolve = GetAnimeInfoResolve(node.OuterHtml);
                animeResolves.Add(animeInfoResolve);

                LogGroup.Log("Fetched: " + animeInfoResolve.Anime.Title);
            }
            LogGroup.Log("\n\n\n");

            return animeResolves;
        }
        public void NextPage()
        {
            var navNode = _doc.DocumentNode.SelectSingleNode("//div[contains(@class,'pagination')]");
            var nextPageLinkNode = navNode.SelectSingleNode(".//li[contains(@class,'next')]/a");

            if (nextPageLinkNode == null)
            {
                IsFinished = true;
                return;
            }

            CurrentPage = AnimeListUrl + nextPageLinkNode.GetAttributeValue("href","");
        }

        private AnimeInfoResolve GetAnimeInfoResolve(string xmlNode)
        {
            var animeResolve = new AnimeInfoResolve();

            var doc = new HtmlDocument();

            doc.LoadHtml(xmlNode);

            var title = doc.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'cardName')]").InnerText;
            animeResolve.Anime.Title = title;

            var detailsUrl = doc.DocumentNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
            animeResolve.AnimeDetailsUrl = WebsiteUrl + detailsUrl;

            var thumbUrl = doc.DocumentNode.SelectSingleNode(".//img").GetAttributeValue("data-src", "");
            animeResolve.AnimeCoverThumbUrl = WebsiteUrl + thumbUrl;

            return animeResolve;
        }
    }
}
