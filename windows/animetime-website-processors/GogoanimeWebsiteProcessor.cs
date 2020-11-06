using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities.Number;
using AnimeTime.Utilities.String;
using AnimeTime.Utilities.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public class GogoanimeWebsiteProcessor : WebsiteProcessor
    {
        protected override char _whiteSpaceDelimiter => '-';
        protected override string _dubAnimeIdentifier => "(Dub)";

        public GogoanimeWebsiteProcessor(string websiteUrl, string querySuffix) : base(websiteUrl, querySuffix)
        {

        }

        public override (string animeUrl, string animeDubUrl) GetAnimeUrl(string animeTitle, int releaseYear, string animeAltTitle = null)
        {
            string animeUrl = null;
            string animeDubUrl = null;

            var searchStrings = GetSearchStrings(animeTitle, animeAltTitle);

            foreach(var searchString in searchStrings)
            {
                var foundAnimes = SearchAnimes(searchString);

                if (foundAnimes.Count() == 0) continue;

                var exactMatch = foundAnimes.FirstOrDefault(t => t.Title.RemoveExtraWhitespaces().Equals(searchString.RemoveExtraWhitespaces(), StringComparison.OrdinalIgnoreCase));

                if (exactMatch != (null, null, 0))
                {
                    animeUrl = exactMatch.Url;

                    var exatchMatchDub = foundAnimes.FirstOrDefault(t => t.Title.RemoveExtraWhitespaces().Equals(searchString.RemoveExtraWhitespaces() + " " + _dubAnimeIdentifier, StringComparison.OrdinalIgnoreCase));

                    if (exatchMatchDub != (null, null, 0))
                    {
                        animeDubUrl = exatchMatchDub.Url;
                    }
                }

                if (foundAnimes.Count() > 2) continue;

                if(foundAnimes.Count() == 1)
                {
                    var match = foundAnimes.ElementAt(0);
                    if(match.releaseYear == releaseYear)
                    {
                        if (match.Title.Contains(_dubAnimeIdentifier))
                        {
                            animeDubUrl = match.Url;
                        }
                        else
                        {
                            animeUrl = match.Url;
                        }
                    }
                }
                else if(foundAnimes.Count() == 2)
                {
                    var first = foundAnimes.ElementAt(0);
                    var second = foundAnimes.ElementAt(1);

                    if(second.Title.Contains(first.Title + " " + _dubAnimeIdentifier))
                    {
                        animeUrl = first.Url;
                        animeDubUrl = second.Url;
                    }
                    else if(first.Title.Contains(second.Title + " " + _dubAnimeIdentifier))
                    {
                        animeUrl = second.Url;
                        animeDubUrl = first.Url;
                    }
                }
            }

            return (animeUrl, animeDubUrl);
        }
        public override IEnumerable<(string Title, string Url, int releaseYear)> SearchAnimes(string searchString)
        {
            var animesFound = new List<(string Title, string Url, int releaseYear)>();

            var doc = _web.Load(UrlUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', _whiteSpaceDelimiter));
            var animeNodes = doc.DocumentNode.SelectNodes(".//div[@class='last_episodes']//ul[@class='items']/li");

            if (animeNodes == null) return animesFound;

            foreach (var node in animeNodes)
            {
                var animeUrlAndTitle = node.SelectSingleNode(".//p[@class='name']/a");

                var title = animeUrlAndTitle.GetAttributeValue("title", null);
                var url = animeUrlAndTitle.GetAttributeValue("href", null);

                var releaseYear = node.SelectSingleNode(".//p[contains(@class,'released')]").InnerText.Trim();

                var releaseYearRegex = new Regex(@"(?<year>\d+)");
                var releaseYearMatch = releaseYearRegex.Match(releaseYear);
                int releaseYearNum = 0;
                if (releaseYearMatch.Success)
                {
                    releaseYearNum = Convert.ToInt32(releaseYearMatch.Groups["year"].Value);
                }

                animesFound.Add((title, url, releaseYearNum));
            }
            return animesFound;
        }
        private IEnumerable<string> GetSearchStrings(string animeTitle, string animeAltTitle = null)
        {
            var searchStrings = new List<string>();

            searchStrings.Add(animeTitle);
            if(animeAltTitle != null)
            {
                searchStrings.Add(animeAltTitle);
            }

            string formatedTitle = animeTitle
                .Replace(": ", " ")
                .Replace('(', ' ')
                .Replace(')', ' ');
            searchStrings.Add(formatedTitle);

            string formatedAltTitle = String.Empty;
            if (animeAltTitle != null)
            {
                formatedAltTitle = animeAltTitle
                    .Replace(": ", " ")
                    .Replace('(', ' ')
                    .Replace(')', ' ');
                searchStrings.Add(formatedAltTitle);
            }

            return searchStrings;
        }

        public override IEnumerable<(int epNum, string epUrl)> GetEpisodes(string animeUrl)
        {
            var episodes = new List<(int epNum, string epUrl)>();

            var doc = _web.Load(animeUrl);

            var lastEpisodePageNode = doc.DocumentNode.SelectSingleNode(".//ul[@id='episode_page']/li[last()]/a");
            if (lastEpisodePageNode == null)
            {
                // Log html change
                return episodes;
            }

            var lastEpisode = lastEpisodePageNode.GetAttributeValue("ep_end", 0);
            if(lastEpisode == 0)
            {
                // Log html change
                return episodes;
            }

            var episodeBaseUrl = animeUrl.Replace("category/", String.Empty).TrimEnd('/');

            for (int i = 1; i <= lastEpisode; i++)
            {
                string epUrl = episodeBaseUrl + "-episode-" + i.ToString();
                episodes.Add((i, epUrl));
            }

            return episodes;
        }
    }
}