
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities.Number;
using AnimeTime.Utilities.String;
using AnimeTime.Utilities.Web;
using AnimeTime.WebsiteProcessors.Models;
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

        public override async Task<AnimeSourceSubDub> TryFindAnime(AnimeSearchParams searchParams)
        {
            var searchStrings = GetSearchStrings(searchParams.Name, searchParams.AltTitles);
            foreach (var searchString in searchStrings)
            {
                var foundAnimes = await SearchAnimesAsync(searchString).ConfigureAwait(false);

                var exactMatch = await TryExactMatch(foundAnimes, searchString);

                if (foundAnimes.Count() == 0) continue;
                else if(exactMatch.IsResolved)
                {
                    return FormatResult(exactMatch);
                }
                else if (foundAnimes.Count() > 2) continue; // Indeterminable
                else if (foundAnimes.Count() == 1)
                {
                    var singleMatch = TryMatchFromSingleResult(foundAnimes, searchParams.ReleaseYear);
                    if (singleMatch.IsResolved)
                        return FormatResult(singleMatch);

                }
                else if (foundAnimes.Count() == 2)
                {
                    var doubleMatch = TryMatchFromDoubleResult(foundAnimes);
                    if (doubleMatch.IsResolved)
                        return FormatResult(doubleMatch);
                }
            }

            return new AnimeSourceSubDub();
        }
        public override async Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString)
        {
            var animesFound = new List<AnimeSearchResult>();

            HtmlDocument doc = null;
            if (CrawlDelayer != null)
            {
                await CrawlDelayer.ApplyDelayAsync(async () => doc = await _web.LoadFromWebAsync(WebUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', WhitespaceDelimiter)).ConfigureAwait(false));
            }
            else
            {
                doc = await _web.LoadFromWebAsync(WebUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', WhitespaceDelimiter)).ConfigureAwait(false);
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

        public override async Task<IEnumerable<EpisodeSource>> GetEpisodesAsync(string animeUrl)
        {
            var episodes = new List<EpisodeSource>();

            var doc = await _web.LoadFromWebAsync(animeUrl).ConfigureAwait(false);

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
                    pageDoc = await _web.LoadFromWebAsync(string.Format(_episodesPageAjaxUrl, episodeStart, episodeEnd, animeId)).ConfigureAwait(false);
                }
                catch (Exception e) // Url is not valid
                {
                    // Log url change

                    return new List<EpisodeSource>();
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
                    episodes.Add(new EpisodeSource() { EpisodeNumber = epNum, Url = epUrl});
                }
            }

            return episodes.OrderBy(e => e.EpisodeNumber);
        }
        public override async Task<IEnumerable<string>> GetVideoSourcesAsync(string episodeUrl)
        {
            var videoSources = new List<string>();

            HtmlDocument doc;
            try
            {
                doc = await _web.LoadFromWebAsync(episodeUrl).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // Log url changed

                return new List<string>();
            }

            var videoSourceNodes = doc.DocumentNode.SelectNodes(".//div[contains(@class,'anime_muti_link')]/ul/li/a");
            if (videoSourceNodes == null)
            {
                // Log html change

                return new List<string>();
            }

            foreach (var sourceNode in videoSourceNodes)
            {
                var source = sourceNode.GetAttributeValue("data-video", null);
                if (source == null)
                {
                    // Log html change

                    return new List<string>();
                }

                videoSources.Add(source);
            }

            return videoSources;
        }

        #region Match attempts
        private async Task<AnimeSourceSubDub> TryExactMatch(IEnumerable<AnimeSearchResult> foundAnimes, string animeName)
        {
            var animeSubDub = new AnimeSourceSubDub();

            var exactMatch = foundAnimes.FirstOrDefault(
                    t => t.Title
                    .Replace(": ", " ")
                    .RemoveExtraWhitespaces()
                    .Equals(animeName.RemoveExtraWhitespaces(), StringComparison.OrdinalIgnoreCase));

            // Check if there is an exact match
            if (exactMatch != null)
            {
                animeSubDub.Sub = new AnimeSource(animeName, exactMatch.Url, AnimeSourceStatus.Resolved);

                var exatchMatchDub = foundAnimes.FirstOrDefault(t => t.Title.RemoveExtraWhitespaces().Equals(animeName.RemoveExtraWhitespaces() + " " + DubAnimeIdentifier, StringComparison.OrdinalIgnoreCase));
                var animeDubUrlCheck = exactMatch.Url + _dubUrlExtension;

                animeSubDub.Dub.Name = $"{animeName} {DubAnimeIdentifier}"; // Assume found
                animeSubDub.Dub.Status_Id = AnimeSourceStatus.Resolved;
                if (exatchMatchDub != null)
                {
                    animeSubDub.Dub.Url = exatchMatchDub.Url;
                    
                }
                else if (await WebUtils.WebpageExistsAsync(WebUtils.CombineUrls(_websiteUrl, animeDubUrlCheck)).ConfigureAwait(false))
                {
                    animeSubDub.Dub.Url = animeDubUrlCheck;
                }
                else
                {
                    animeSubDub.Dub.Status_Id = AnimeSourceStatus.CouldNotResolve;
                }
            }

            return animeSubDub;
        }
        private AnimeSourceSubDub TryMatchFromSingleResult(IEnumerable<AnimeSearchResult> foundAnimes, int? releaseYear)
        {
            var animeSubDub = new AnimeSourceSubDub();

            var match = foundAnimes.ElementAt(0);
            if (match.ReleaseYear == releaseYear)
            {
                if (match.Title.Contains(DubAnimeIdentifier))
                {
                    animeSubDub.Dub = new AnimeSource(match.Title, match.Url, AnimeSourceStatus.Resolved);
                }
                else
                {
                    animeSubDub.Sub = new AnimeSource(match.Title, match.Url, AnimeSourceStatus.Resolved);
                }
            }
            return animeSubDub;
        }
        private AnimeSourceSubDub TryMatchFromDoubleResult(IEnumerable<AnimeSearchResult> foundAnimes)
        {
            var animeSourceSubDub = new AnimeSourceSubDub();

            var first = foundAnimes.ElementAt(0);
            var second = foundAnimes.ElementAt(1);

            if (second.Title.Contains(first.Title + " " + DubAnimeIdentifier))
            {
                animeSourceSubDub.Sub = new AnimeSource(first.Title, first.Url, AnimeSourceStatus.Resolved);
                animeSourceSubDub.Dub = new AnimeSource(second.Title, second.Url, AnimeSourceStatus.Resolved);
            }
            else if (first.Title.Contains(second.Title + " " + DubAnimeIdentifier))
            {
                animeSourceSubDub.Sub = new AnimeSource(second.Title, second.Url, AnimeSourceStatus.Resolved);
                animeSourceSubDub.Dub = new AnimeSource(first.Title, first.Url, AnimeSourceStatus.Resolved);
            }

            return animeSourceSubDub;
        }
        #endregion

        #region Result
        private AnimeSourceSubDub FormatResult(AnimeSourceSubDub animeSourceSubDub)
        {
            if (animeSourceSubDub.Sub.Url != null)
                animeSourceSubDub.Sub.Url = WebUtils.CombineUrls(_websiteUrl, animeSourceSubDub.Sub.Url);

            if (animeSourceSubDub.Dub.Url != null)
                animeSourceSubDub.Dub.Url = WebUtils.CombineUrls(_websiteUrl, animeSourceSubDub.Dub.Url);

            return animeSourceSubDub;
        }
        #endregion
    }
}