
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
        

        protected override char WhitespaceDelimiter => '-';

        public GogoanimeWebsiteProcessor(string websiteUrl, string querySuffix) : base(websiteUrl, querySuffix)
        {

        }

        protected override int GetSearchItemReleaseYear(HtmlNode node)
        {
            var releaseYear = node.SelectSingleNode(".//p[contains(@class,'released')]").InnerText.Trim();

            var releaseYearRegex = new Regex(@"(?<year>\d+)");
            var releaseYearMatch = releaseYearRegex.Match(releaseYear);
            int releaseYearNum = 0;
            if (releaseYearMatch.Success)
            {
                releaseYearNum = Convert.ToInt32(releaseYearMatch.Groups["year"].Value);
            }

            return releaseYearNum;
        }
        protected override string GetSearchItemUrl(HtmlNode searchItem)
        {
            var urlNode = searchItem.SelectSingleNode(".//a");
            return urlNode.GetAttributeValue("href", null);
        }
        protected override string GetSearchItemTitle(HtmlNode searchItem)
        {
            var titleNode = searchItem.SelectSingleNode(".//a");
            return titleNode.GetAttributeValue("title", null);
        }
        protected override HtmlNodeCollection GetSearchItemNodes(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes(".//div[@class='last_episodes']//ul[@class='items']/li");
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
                    episodes.Add(new EpisodeSource() { EpisodeNumber = epNum, Url = epUrl });
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
    }
}