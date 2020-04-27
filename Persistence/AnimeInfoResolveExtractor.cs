using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimeInfoResolveExtractor : IAnimeInfoResolveExtractor
    {
        private HtmlWeb _web;
        private HtmlDocument _doc;

        public bool IsFinished { get; set; } = false;
        public string CurrentPage { get; set; }
        public string RootUrl { get ; set ; }
        public string AnimeListRootUrl { get ; set ; }

        public AnimeInfoResolveExtractor(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;
        }

        private AnimeInfoResolve GetAnimeInfoResolve(string xmlNode)
        {
            var animeResolve = new AnimeInfoResolve();

            var doc = new HtmlDocument();

            doc.LoadHtml(xmlNode);

            var title = doc.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'cardName')]").InnerText;
            animeResolve.Anime.Title = title;

            var detailsUrl = doc.DocumentNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
            animeResolve.AnimeDetailsUrl = RootUrl + detailsUrl;

            var thumbUrl = doc.DocumentNode.SelectSingleNode(".//img").GetAttributeValue("data-src", "");
            animeResolve.AnimeCoverThumbUrl = RootUrl + thumbUrl;

            return animeResolve;
        }
        public IEnumerable<AnimeInfoResolve> GetAnimeInfoResolvesFromPage()
        {
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

            CurrentPage = AnimeListRootUrl + nextPageLinkNode.GetAttributeValue("href","");
        }
    }
}
