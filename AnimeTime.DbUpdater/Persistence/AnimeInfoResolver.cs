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
using AnimeTime.Utilities.Web;

namespace AnimeTimeDbUpdater.Persistence
{
    public class AnimeInfoResolver : IAnimeInfoResolver
    {
        private static string _blankCover = "blank_main.jpg";

        private HtmlWeb _web;
        private HtmlDocument _doc;

        public ICrawlDelayer CrawlDelayer { get; set; }

        public AnimeInfoResolver(HtmlWeb web, HtmlDocument doc)
        {
            _web = web;
            _doc = doc;
        }

        public AnimeDetailedInfo Resolve(AnimeBasicInfo basicInfo)
        {
            var detailedInfo = new AnimeDetailedInfo();

            if(CrawlDelayer != null)
            {
                CrawlDelayer.ApplyDelay(() => { _doc = _web.Load(basicInfo.DetailsUrl); });
            }
            else
            {
                _doc = _web.Load(basicInfo.DetailsUrl);
            }

            detailedInfo.BasicInfo = basicInfo;
            detailedInfo.AltTitles = new List<string>(ResolveAltTitles());
            detailedInfo.Description = ResolveDescription();
            detailedInfo.ReleaseYear = ResolveYear();
            detailedInfo.YearSeason = ResolveYearSeason();
            detailedInfo.Rating = ResolveRating();
            detailedInfo.Category = ResolveCategory();
            detailedInfo.Genres = new List<string>(ResolveGenres());
            detailedInfo.CharactersUrl = GetCharactersUrl();
            detailedInfo.CoverUrl = GetCoverUrl();

            return detailedInfo;
        }

        private IEnumerable<string> ResolveAltTitles()
        {
            var altTitles = new List<string>();
            var titleNode = _doc.DocumentNode.SelectSingleNode("//h2[contains(@class,'aka')]");
            if (titleNode != null)
            {
                var titleNodeText = titleNode.InnerText.Replace("\n", String.Empty);

                var titles = titleNodeText.Split(new string[] { "Alt title: ", "Alt titles: ", "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var title in titles)
                {
                    var altTitle = HttpUtility.HtmlDecode(title);
                    altTitle = altTitle.Trim();

                    altTitles.Add(altTitle);
                }
            }

            return altTitles;
        }
        private string ResolveDescription()
        {
            var descriptionNode = _doc.DocumentNode.SelectSingleNode(".//section[@id='entry']//div[contains(@class,'entrySynopsis')]/div[2]/div[1]/p[1]");
            if (descriptionNode == null) return null;

            return HttpUtility.HtmlDecode(descriptionNode.InnerText);
        }
        private int? ResolveYear()
        {
            var yearInnerText = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").InnerText;
            var year = yearInnerText.Split('-')[0];
            year = year.Trim();
            if (Int32.TryParse(year, out int parsedYear))
            {
                return parsedYear;
            }
            else
            {
#if DEBUG
                Log.TraceEvent(TraceEventType.Error, 0, $"[Exception caught]: Could not parse release year. Value: {year}");
#endif
                return null;
            }
        }
        private string ResolveYearSeason()
        {
            var yearSeasonNode = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'iconYear')]").ParentNode.SelectSingleNode(".//a");
            if (yearSeasonNode == null) return null;

            var yearSeason = yearSeasonNode.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            return yearSeason;
        }
        private float ResolveRating()
        {
            var ratingNode = _doc.DocumentNode.SelectSingleNode(".//section[contains(@class,'entryBar')]//div[contains(@class,'avgRating')]/span");

            if (ratingNode == null)
            {
                // Log html change

                return 0F;
            }

            var styleAttr = ratingNode.GetAttributeValue("style", null);
            if(styleAttr == null)
            {
                // Log html change

                return 0F;
            }
            if (styleAttr.Contains("0%")) return 0F;

            float ratingValue = Convert.ToSingle(ratingNode.InnerText.Split(' ')[0]);
            return ratingValue;
        }
        private string ResolveCategory()
        {
            var category = _doc.DocumentNode.SelectSingleNode("//span[contains(@class,'type')]").InnerText;
            return category.Split('(')[0].Trim();

        }
        private IEnumerable<string> ResolveGenres()
        {
            var genres = new List<string>();
            var tagListContainers = _doc.DocumentNode.SelectNodes("//div[contains(@class,'tags')]");
            HtmlNode tagListContainer;
            if (tagListContainers == null) return genres;

            tagListContainer = tagListContainers[0];

            var tags = tagListContainer.SelectNodes(".//li/a[contains(@data-tooltip,'tags')]");

            foreach (var tag in tags)
            {
                var genreName = tag.InnerText.Replace("\n", String.Empty).Trim();
                genres.Add(genreName);
            }

            return genres;
        }
        private string GetCharactersUrl()
        {
            var charactersLinkNode = _doc.DocumentNode.SelectSingleNode("//ul[@class='subNav']/li/a[contains(text(),'characters')]");

            string charactersUrl = String.Empty;

            if (charactersLinkNode != null)
                charactersUrl = charactersLinkNode.GetAttributeValue("href", String.Empty);

            return Constants.WebsiteUrls.AnimePlanet + charactersUrl;
        }
        private string GetCoverUrl()
        {
            var coverNode = _doc.DocumentNode.SelectSingleNode("//section[@id='entry']//div[@class='mainEntry']//img[@itemprop='image']");

            if (coverNode == null) // Error fetching node
            {
                // Log exception (log document content, and searched node)
                return null;
            }

            var coverSrc = coverNode.GetAttributeValue("src", null);

            if (coverSrc == null || coverSrc.Contains(_blankCover))
            {
                return null;
            }

            if (UrlUtil.IsAbsolute(coverSrc))
                return coverSrc;
            else
                return Constants.WebsiteUrls.AnimePlanet + coverSrc;
        }
    }
}