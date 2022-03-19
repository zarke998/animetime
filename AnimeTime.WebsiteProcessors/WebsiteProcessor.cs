using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities;
using AnimeTime.Utilities.String;
using AnimeTime.Utilities.Web;
using AnimeTime.WebsiteProcessors.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public abstract class WebsiteProcessor : IWebsiteProcessor
    {
        protected string _websiteUrl;
        protected string _querySuffix;

        protected HtmlWeb _web;

        protected virtual string EpisodeUrlPrefix => String.Empty;
        protected virtual char WhitespaceDelimiter { get; } = ' ';
        protected virtual string DubAnimeIdentifier { get; } = "(Dub)";
        protected virtual string DubUrlExtension => "-dub";

        public ICrawlDelayer CrawlDelayer { get; set; }

        public WebsiteProcessor(string websiteUrl, string querySuffix)
        {
            this._websiteUrl = websiteUrl;
            this._querySuffix = querySuffix;

            this._web = new HtmlWeb();
        }

        #region IWebsiteProcessor
        public virtual async Task<AnimeSourceSubDub> TryFindAnime(AnimeSearchParams searchParams)
        {
            var searchStrings = GetSearchStrings(searchParams.Name, searchParams.AltTitles);
            foreach (var searchString in searchStrings)
            {
                var foundAnimes = await SearchAnimesAsync(searchString).ConfigureAwait(false);

                var exactMatch = await TryExactMatch(foundAnimes, searchString);

                if (foundAnimes.Count() == 0) continue;
                else if (exactMatch.IsResolved)
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
        public virtual async Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString)
        {
            var animesFound = new List<AnimeSearchResult>();

            HtmlDocument doc = null;
            if (CrawlDelayer != null)
            {
                await CrawlDelayer.ApplyDelayAsync(async () => doc = await _web.LoadFromWebAsync(WebUtils.CombineUrls(_websiteUrl, _querySuffix) + searchString.Replace(' ', WhitespaceDelimiter)).ConfigureAwait(false));
            }
            else
            {
                doc = await _web.LoadFromWebAsync(_websiteUrl + _querySuffix + searchString.Replace(' ', WhitespaceDelimiter)).ConfigureAwait(false);
            }

            var animeNodes = GetSearchItemNodes(doc);
            if (animeNodes == null) return animesFound;

            foreach (var node in animeNodes)
            {
                animesFound.Add(new AnimeSearchResult()
                {
                    Title = GetSearchItemTitle(node),
                    Url = GetSearchItemUrl(node),
                    ReleaseYear = GetSearchItemReleaseYear(node)
                });
            }
            return animesFound;
        }
        public virtual async Task<IEnumerable<EpisodeSource>> GetEpisodesAsync(string animeUrl)
        {
            var episodes = new List<EpisodeSource>();

            var doc = await _web.LoadFromWebAsync(animeUrl).ConfigureAwait(false);

            var lastEpisode = GetLastEpisode(doc);
            for (int i = 1; i <= lastEpisode; i++)
            {
                episodes.Add(new EpisodeSource() { EpisodeNumber = i, Url = $"{EpisodeUrlPrefix}{FormatEpisodeNumber(i)}" });
            }

            return episodes.OrderBy(e => e.EpisodeNumber);
        }
        public abstract Task<IEnumerable<string>> GetVideoSourcesAsync(string episodeUrl);
        #endregion

        protected virtual IEnumerable<string> GetSearchStrings(string animeTitle, IEnumerable<string> animeAltTitles)
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

        #region Match attempts
        protected virtual async Task<AnimeSourceSubDub> TryExactMatch(IEnumerable<AnimeSearchResult> foundAnimes, string animeName)
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
                var animeDubUrlCheck = exactMatch.Url + DubUrlExtension;

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
        protected virtual AnimeSourceSubDub TryMatchFromSingleResult(IEnumerable<AnimeSearchResult> foundAnimes, int? releaseYear)
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
        protected virtual AnimeSourceSubDub TryMatchFromDoubleResult(IEnumerable<AnimeSearchResult> foundAnimes)
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
        protected virtual AnimeSourceSubDub FormatResult(AnimeSourceSubDub animeSourceSubDub)
        {
            if (animeSourceSubDub.Sub.Url != null)
                animeSourceSubDub.Sub.Url = WebUtils.CombineUrls(_websiteUrl, animeSourceSubDub.Sub.Url);

            if (animeSourceSubDub.Dub.Url != null)
                animeSourceSubDub.Dub.Url = WebUtils.CombineUrls(_websiteUrl, animeSourceSubDub.Dub.Url);

            return animeSourceSubDub;
        }
        #endregion

        #region XPath mappings
        protected abstract int GetSearchItemReleaseYear(HtmlNode node);
        protected abstract string GetSearchItemUrl(HtmlNode searchItem);
        protected abstract string GetSearchItemTitle(HtmlNode searchItem);
        protected abstract HtmlNodeCollection GetSearchItemNodes(HtmlDocument doc);
        #endregion

        protected virtual string FormatEpisodeNumber(int number)
        {
            return number.ToString();
        }
        protected virtual int GetLastEpisode(HtmlDocument doc)
        {
            return 0;
        }

    }
}