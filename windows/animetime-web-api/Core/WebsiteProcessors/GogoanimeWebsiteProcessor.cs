using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities.Number;
using AnimeTime.Utilities.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AnimeTime.WebAPI.Core.WebsiteProcessors
{
    public class GogoanimeWebsiteProcessor : WebsiteProcessor
    {
        protected override char _whiteSpaceDelimiter => '-';
        protected override string _dubAnimeIdentifier => "(Dub)";

        public GogoanimeWebsiteProcessor(Website website) : base(website)
        {

        }

        public override string GetAnimeUrl(string animeTitle, string animeAltTitle = null, CategoryId? category = null)
        {
            var searchStrings = GetSearchStrings(animeTitle, animeAltTitle, category);

            // Check if anime has only dub version (ex. Fullmetal Alchemist: Brotherhood Specials (Dub))
        }

        public override IEnumerable<(string Title, string Url)> SearchAnimes(string searchString)
        {
            var animesFound = new List<(string Title, string Url)>();

            var doc = _web.Load(UrlUtils.CombineUrls(_website.Url, _website.QuerySuffix) + searchString.Replace(' ', _whiteSpaceDelimiter));
            var animeNodes = doc.DocumentNode.SelectNodes(".//div[@class='last_episodes']//ul[@class='items']/li");

            if (animeNodes == null) return animesFound;

            foreach (var node in animeNodes)
            {
                var animeUrlAndTitle = node.SelectSingleNode(".//p[@class='name']/a");

                var title = animeUrlAndTitle.GetAttributeValue("title", null);
                var url = animeUrlAndTitle.GetAttributeValue("href", null);

                animesFound.Add((title, url));
            }
            return animesFound;
        }

        private IEnumerable<string> GetSearchStrings(string animeTitle, string animeAltTitle = null, CategoryId? category = null)
        {
            var searchStrings = new List<string>();

            string titleWithoutColon = animeTitle.Replace(": ", " ");
            searchStrings.Add(titleWithoutColon);

            string altTitleWithoutColon = String.Empty;
            if (animeAltTitle != null)
            {
                altTitleWithoutColon = animeAltTitle.Replace(": ", " ");
                searchStrings.Add(animeAltTitle);
            }

            // Search without long name for special episodes (ex. Steins;Gate 0: Valentine's of Crystal Polymorphism -Bittersweet Intermedio- -> Steins;Gate 0 Special)
            if (category != null && category == CategoryId.DVD_Special)
            {
                var splittedTitle = animeTitle.Split(':');

                if (splittedTitle.Length > 1)
                {
                    searchStrings.Add(splittedTitle[0].Trim() + " Special");
                }
            }

            // Search without unnecessary character for movies (ex. Re:Zero kara Hajimeru Isekai Seikatsu[:] Memory Snow)  
            if (category != null && category == CategoryId.Movie)
            {
                if (animeAltTitle != null)
                {
                    searchStrings.Add(animeAltTitle.Replace(": ", " "));
                }
                searchStrings.Add(animeTitle.Replace(": ", " "));
            }


            // Search with different season search string (ex 3rd Season -> Season 3)
            var formatedTitleSeason = ChangeSeasonFormat(titleWithoutColon);
            if(formatedTitleSeason != titleWithoutColon)
            {
                searchStrings.Add(formatedTitleSeason);
            }

            if(animeAltTitle != null)
            {
                var formatedAltTitleSeason = ChangeSeasonFormat(altTitleWithoutColon);
                if(formatedAltTitleSeason != altTitleWithoutColon)
                {
                    searchStrings.Add(formatedAltTitleSeason);
                }
            }

            string ChangeSeasonFormat(string input)
            {
                var formatedInput = input;

                var seasonRegex = new Regex(@"(?<digit>\d)(st|nd|rd|th) Season");
                var seasonMatch = seasonRegex.Match(input);
                if (seasonMatch.Success)
                {
                    var seasonNum = seasonMatch.Groups["digit"].Value;

                    formatedInput = seasonRegex.Replace(input, $"Season {seasonNum}");
                }

                var partRegex = new Regex(@"Part (?<romanDigit>\w+)");
                var partMatch = partRegex.Match(formatedInput);
                if (partMatch.Success)
                {
                    var partNumRoman = partMatch.Groups["romanDigit"].Value;

                    var partNum = RomanToArabic.Convert(partNumRoman);
                    formatedInput = seasonRegex.Replace(formatedInput, $"Part {partNum}");
                }

                return formatedInput;
            }


        }
    }
}