using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Persistence;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using HtmlAgilityPack;

namespace AnimeTimeDbUpdater.Persistence
{
    public class AnimeInfoResolver : IAnimeInfoResolver
    {
        private HtmlWeb _web;

        public AnimeInfoResolver(HtmlWeb web)
        {
            _web = web;
        }

        public Anime Resolve(AnimeInfoResolve animeInfoResolve)
        {
            Anime resolvedAnime = animeInfoResolve.Anime;

            resolvedAnime.CoverThumb = GetImageFromUrl(animeInfoResolve.AnimeCoverThumbUrl);
            ResolveDetails(animeInfoResolve);

            return resolvedAnime;
        }

        private byte[] GetImageFromUrl(string thumbUrl)
        {
            using (WebClient client = new WebClient())
                return client.DownloadData(thumbUrl);
        }

        private void ResolveDetails(AnimeInfoResolve animeInfoResolve)
        {
            var anime = animeInfoResolve.Anime;

            HtmlDocument doc = _web.Load(animeInfoResolve.AnimeDetailsUrl);

            // Extract alt title
            var titleNode = doc.DocumentNode.SelectSingleNode("//h2[contains(@class,'aka')]");
            if (titleNode != null)
            {
                var title = titleNode.InnerText;

                var titleSplited = title.Split(new string[] { "Alt title: " }, StringSplitOptions.RemoveEmptyEntries);
                title = titleSplited[1];
                anime.TitleAlt = title.Replace("\n", String.Empty);
            }

            //Extract description
            var description = doc.DocumentNode.SelectSingleNode("//div[contains(@itemprop,'description')]/p").InnerText;
            anime.Description = description;

            //Extract year
            var yearInnerText = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").InnerText;
            var year = yearInnerText.Split('-')[0];
            year = year.Trim();
            anime.Year = new Year() { Name = year };

            //Extract year season
            var yearSeasonNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").ParentNode.SelectSingleNode(".//a");
            if (yearSeasonNode != null)
            {
                var yearSeason = yearSeasonNode.InnerText;
                anime.YearSeason = new YearSeason() { YearSeasonName = yearSeason };
            }

            //Extract rating
            var ratingNode = doc.DocumentNode.SelectSingleNode("//meta[contains(@itemprop,'ratingValue')]");
            if (ratingNode != null)
            {
                float ratingValue = Convert.ToSingle(ratingNode.GetAttributeValue("content", "0"));
                anime.Rating = ratingValue;
            }
            else
                anime.Rating = 0F;

            //Extract category
            var category = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'type')]").InnerText;
            category = category.Split('(')[0].Trim();
            anime.Category = new Category() { Name = category };

            //Extract genres
            var tagListContainer = doc.DocumentNode.SelectNodes("//div[contains(@class,'tags')]")[0];
            var tags = tagListContainer.SelectNodes(".//li[contains(@itemprop,'genre')]/a");

            foreach(var tag in tags)
                anime.Genres.Add(new Genre() { Name = tag.InnerText.Replace("\n", String.Empty) });
            

        }
    }
}
