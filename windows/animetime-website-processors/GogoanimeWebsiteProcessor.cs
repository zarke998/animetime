using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities.Number;
using AnimeTime.Utilities.String;
using AnimeTime.Utilities.Web;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public class GogoanimeWebsiteProcessor : WebsiteProcessor
    {
        private string _episodesPageAjaxUrl = "https://ajax.gogocdn.net/ajax/load-list-episode?ep_start={0}&ep_end={1}&id={2}&default_ep=0";
        private readonly string _dubUrlExtension = "-dub";

        protected override char WhitespaceDelimiter => '-';
        protected override string DubAnimeIdentifier => "(Dub)";

        public GogoanimeWebsiteProcessor(string websiteUrl, string querySuffix) : base(websiteUrl, querySuffix)
        {

        }

        public override async Task<(string animeUrl, string animeDubUrl)> GetAnimeUrlAsync(string animeTitle, int? releaseYear, IEnumerable<string> animeAltTitles)
        {
            string animeUrl = null;
            string animeDubUrl = null;

            var searchStrings = GetSearchStrings(animeTitle, animeAltTitles);

            var resultFound = false;
            foreach (var searchString in searchStrings)
            {
                var foundAnimes = await SearchAnimesAsync(searchString);

                if (foundAnimes.Count() == 0) continue;

                var exactMatch = foundAnimes.FirstOrDefault(
                    t => t.Title
                    .Replace(": ", " ")
                    .RemoveExtraWhitespaces()
                    .Equals(searchString.RemoveExtraWhitespaces(), StringComparison.OrdinalIgnoreCase));

                // Check if there is an exact match
                if (exactMatch != null)
                {
                    animeUrl = exactMatch.Url;
                    resultFound = true;

                    var exatchMatchDub = foundAnimes.FirstOrDefault(t => t.Title.RemoveExtraWhitespaces().Equals(searchString.RemoveExtraWhitespaces() + " " + DubAnimeIdentifier, StringComparison.OrdinalIgnoreCase));
                    var animeDubUrlCheck = animeUrl + _dubUrlExtension;

                    if (exatchMatchDub != null)
                    {
                        animeDubUrl = exatchMatchDub.Url;
                    }
                    else if (await WebUtils.WebpageExistsAsync(WebUtils.CombineUrls(_websiteUrl, animeDubUrlCheck)))
                    {
                        animeDubUrl = animeDubUrlCheck;
                    }
                }
                else if (foundAnimes.Count() > 2) continue;
                else if (foundAnimes.Count() == 1) 
                {
                    var match = foundAnimes.ElementAt(0);
                    if (match.ReleaseYear == releaseYear)
                    {
                        if (match.Title.Contains(DubAnimeIdentifier))
                        {
                            animeDubUrl = match.Url;
                        }
                        else
                        {
                            animeUrl = match.Url;
                        }
                        resultFound = true;
                    }
                }
                else if (foundAnimes.Count() == 2)
                {
                    var first = foundAnimes.ElementAt(0);
                    var second = foundAnimes.ElementAt(1);

                    if (second.Title.Contains(first.Title + " " + DubAnimeIdentifier))
                    {
                        animeUrl = first.Url;
                        animeDubUrl = second.Url;
                        resultFound = true;
                    }
                    else if (first.Title.Contains(second.Title + " " + DubAnimeIdentifier))
                    {
                        animeUrl = second.Url;
                        animeDubUrl = first.Url;
                        resultFound = true;
                    }
                }

                if (resultFound) break;
            }

            if (animeUrl != null) animeUrl = WebUtils.CombineUrls(_websiteUrl, animeUrl);
            if (animeDubUrl != null) animeDubUrl = WebUtils.CombineUrls(_websiteUrl, animeDubUrl);

            return (animeUrl, animeDubUrl);
        }
        public override async Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString)
        {
            var animesFound = new List<AnimeSearchResult>();

            HtmlDocument doc = null;
            if (CrawlDelayer != null)
            {
                await CrawlDelayer.ApplyDelayAsync(async () => doc = await _web.LoadFromWebAsync(WebUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', WhitespaceDelimiter)));
            }
            else
            {
                doc = await _web.LoadFromWebAsync(WebUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', WhitespaceDelimiter));
            }

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

                animesFound.Add(new AnimeSearchResult()
                {
                    Title = title,
                    Url = url,
                    ReleaseYear = releaseYearNum
                });
            }
            return animesFound;
        }
        private IEnumerable<string> GetSearchStrings(string animeTitle, IEnumerable<string> animeAltTitles)
        {
            var searchStrings = new List<string>();

            searchStrings.Add(animeTitle);
            foreach (var altTitle in animeAltTitles)
            {
                searchStrings.Add(altTitle);
            }

            string formatedTitle = animeTitle
                .Replace(": ", " ")
                .Replace('(', ' ')
                .Replace(')', ' ');
            searchStrings.Add(formatedTitle);

            foreach (var altTitle in animeAltTitles)
            {
                var formatedAltTitle = altTitle
                    .Replace(": ", " ")
                    .Replace('(', ' ')
                    .Replace(')', ' ');

                if (formatedAltTitle != altTitle)
                    searchStrings.Add(formatedAltTitle);
            }

            return searchStrings;
        }

        public override IEnumerable<(float epNum, string epUrl)> GetEpisodes(string animeUrl)
        {
            var episodes = new List<(float epNum, string epUrl)>();

            var doc = _web.Load(animeUrl);

            var animeIdNode = doc.DocumentNode.SelectSingleNode(".//input[@id='movie_id']");
            if (animeIdNode == null)
            {
                // Log html change

                return episodes;
            }
            var animeId = Convert.ToInt32(animeIdNode.GetAttributeValue("value", null));

            var episodePages = doc.DocumentNode.SelectNodes(".//ul[@id='episode_page']/li/a");
            if (episodePages == null)
            {
                // Log html change
                return episodes;
            }

            foreach (var page in episodePages)
            {
                var episodeStart = page.GetAttributeValue("ep_start", null);
                var episodeEnd = page.GetAttributeValue("ep_end", null);

                if (episodeStart == null || episodeEnd == null)
                {
                    // Log html change

                    return episodes;
                }

                HtmlDocument pageDoc;
                try
                {
                    pageDoc = _web.Load(string.Format(_episodesPageAjaxUrl, episodeStart, episodeEnd, animeId));
                }
                catch (Exception e) // Url is not valid
                {
                    // Log url change

                    return new List<(float, string)>();
                }

                var episodeNodes = pageDoc.DocumentNode.SelectNodes(".//ul/li/a/div[contains(@class,'name')]");
                foreach (var episodeNode in episodeNodes)
                {
                    var episodeText = episodeNode.InnerText.RemoveExtraWhitespaces().Split(' ')[1];
                    var epNum = Convert.ToSingle(episodeText);

                    var epNumWhole = Convert.ToInt32(epNum);
                    var epNumDecimal = Convert.ToInt32((epNum - epNumWhole) * 10);

                    var epUrl = animeUrl.Replace("category/", String.Empty) + "-episode-";
                    epUrl += epNumWhole.ToString();
                    if (epNumDecimal != 0)
                    {
                        epUrl += string.Format("-{0}", epNumDecimal);
                    }
                    episodes.Add((epNum, epUrl));
                }
            }

            return episodes.OrderBy(e => e.epNum);
        }
    }
}