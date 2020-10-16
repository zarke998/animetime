using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using HtmlAgilityPack;
using System.Web;
using System.Diagnostics;
using AnimeTime.Utilities;

namespace AnimeTimeDbUpdater.Persistence
{
    public class AnimeInfoResolver : IAnimeInfoResolver
    {
        private HtmlWeb _web;
        private HtmlDocument _doc;

        public AnimeInfoResolver(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;
        }

        public void Resolve(AnimeInfo info)
        {
            var anime = info.Anime;


            CrawlDelayer.ApplyDelay(() => { _doc = _web.Load(info.AnimeDetailsUrl); });

            ResolveAltTitle(anime);
            ResolveDescription(anime);
            ResolveYear(anime);
            ResolveYearSeason(anime);
            ResolveRating(anime);
            ResolveCategory(anime);
            ResolveGenres(anime);
            info.CharactersUrl = GetCharactersUrl();

            LogGroup.Log($"Resolved: {info.AnimeDetailsUrl} ({anime.Title})");
        }

        private void ResolveAltTitle(Anime anime)
        {
            var titleNode = _doc.DocumentNode.SelectSingleNode("//h2[contains(@class,'aka')]");
            if (titleNode != null)
            {
                var titleNodeText = titleNode.InnerText.Replace("\n", String.Empty);

                var titles = titleNodeText.Split(new string[] { "Alt title: ","Alt titles: ", "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var title in titles)
                {
                    var altTitle = HttpUtility.HtmlDecode(title);
                    altTitle = altTitle.Trim();

                    anime.AltTitles.Add(new AnimeAltTitle() { Title = altTitle });
                }
            }
        }
        private void ResolveDescription(Anime anime)
        {
            var descriptionNode = _doc.DocumentNode.SelectSingleNode("//div[contains(@itemprop,'description')]/p");
            if (descriptionNode != null)
                anime.Description = HttpUtility.HtmlDecode(descriptionNode.InnerText);

        }
        private void ResolveYear(Anime anime)
        {
            var yearInnerText = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").InnerText;
            var year = yearInnerText.Split('-')[0];
            year = year.Trim();
            if (Int32.TryParse(year, out int parsedYear))
            {
                anime.ReleaseYear = parsedYear;
            }
            else
            {
#if DEBUG
                Console.WriteLine($"[Exception caught]: Could not parse release year. Value: {year}");
#endif
                anime.ReleaseYear = null;
            }
        }
        private void ResolveYearSeason(Anime anime)
        {
            var yearSeasonNode = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").ParentNode.SelectSingleNode(".//a");
            if (yearSeasonNode != null)
            {
                var yearSeason = yearSeasonNode.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                anime.YearSeason = new YearSeason() { Name = yearSeason };
            }
        }
        private void ResolveRating(Anime anime)
        {
            var ratingNode = _doc.DocumentNode.SelectSingleNode("//meta[contains(@itemprop,'ratingValue')]");
            if (ratingNode != null)
            {
                float ratingValue = Convert.ToSingle(ratingNode.GetAttributeValue("content", "0"));
                anime.Rating = ratingValue;
            }
            else
                anime.Rating = 0F;
        }
        private void ResolveCategory(Anime anime)
        {
            var category = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'type')]").InnerText;
            category = category.Split('(')[0].Trim();
            anime.Category = new Category() { Name = category };
        }
        private void ResolveGenres(Anime anime)
        {
            var tagListContainers = _doc.DocumentNode.SelectNodes("//div[contains(@class,'tags')]");
            HtmlNode tagListContainer;
            if (tagListContainers == null)
                return;
            else
                tagListContainer = tagListContainers[0];

            var tags = tagListContainer.SelectNodes(".//li/a[contains(@data-tooltip,'tags')]");

            foreach (var tag in tags)
            {
                var genreName = tag.InnerText.Replace("\n", String.Empty).Trim();
                anime.Genres.Add(new Genre() { Name = genreName });
            }
        }
        private string GetCharactersUrl()
        {
            var charactersLinkNode = _doc.DocumentNode.SelectSingleNode("//ul[@class='subNav']/li/a[contains(text(),'characters')]");

            string charactersUrl = String.Empty;

            if(charactersLinkNode != null)
                charactersUrl = charactersLinkNode.GetAttributeValue("href", String.Empty);

            return Constants.WebsiteUrls.AnimePlanet + charactersUrl;
        }
    }
}