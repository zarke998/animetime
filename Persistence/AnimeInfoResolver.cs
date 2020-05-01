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

        public Anime Resolve(AnimeInfoResolve animeInfoResolve)
        {
            var anime = animeInfoResolve.Anime;

            //resolvedAnime.CoverThumb = GetImageFromUrl(animeInfoResolve.AnimeCoverThumbUrl);
            anime.CoverThumbUrl = animeInfoResolve.AnimeCoverThumbUrl;

            CrawlStopwatch.ApplyDelay();

            CrawlStopwatch.BeginCrawlTracking();
            _doc = _web.Load(animeInfoResolve.AnimeDetailsUrl);
            CrawlStopwatch.EndCrawlTracking();


            ResolveAltTitle(anime);
            ResolveDescription(anime);
            ResolveYear(anime);
            ResolveYearSeason(anime);
            ResolveRating(anime);
            ResolveCategory(anime);
            ResolveGenres(anime);

            LogGroup.Log($"Resolved: {animeInfoResolve.AnimeDetailsUrl} ({anime.Title})");
            return anime;
        }
        private byte[] GetImageFromUrl(string thumbUrl)
        {
            using (WebClient client = new WebClient())
                return client.DownloadData(thumbUrl);
        }

        private void ResolveAltTitle(Anime anime)
        {
            var titleNode = _doc.DocumentNode.SelectSingleNode("//h2[contains(@class,'aka')]");
            if (titleNode != null)
            {
                var title = titleNode.InnerText;

                var titleSplited = title.Split(new string[] { "Alt title: " }, StringSplitOptions.RemoveEmptyEntries);
                title = titleSplited[1];
                anime.TitleAlt = title.Replace("\n", String.Empty);
            }
        }
        private void ResolveDescription(Anime anime) 
        {
            var description = _doc.DocumentNode.SelectSingleNode("//div[contains(@itemprop,'description')]/p").InnerText;
            anime.Description = HttpUtility.HtmlDecode(description);
            
        }
        private void ResolveYear(Anime anime)
        {
            var yearInnerText = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").InnerText;
            var year = yearInnerText.Split('-')[0];
            year = year.Trim();
            anime.ReleaseYear = Convert.ToInt32(year);
        }
        private void ResolveYearSeason(Anime anime)
        {
            var yearSeasonNode = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").ParentNode.SelectSingleNode(".//a");
            if (yearSeasonNode != null)
            {
                var yearSeason = yearSeasonNode.InnerText;
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
            var tagListContainer = _doc.DocumentNode.SelectNodes("//div[contains(@class,'tags')]")[0];
            var tags = tagListContainer.SelectNodes(".//li[contains(@itemprop,'genre')]/a");

            foreach (var tag in tags)
            {
                var genreName = tag.InnerText.Replace("\n", String.Empty).Trim();
                anime.Genres.Add(new Genre() { Name = genreName});
            }
        }
    }
}