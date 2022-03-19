using AnimeTime.WebsiteProcessors.AjaxProcessors;
using AnimeTime.WebsiteProcessors.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors
{
    public class AnimeOwlWebsiteProcessor : WebsiteProcessor
    {
        public AnimeOwlWebsiteProcessor(string websiteUrl, string querySuffix) : base(websiteUrl, querySuffix)
        {

        }

        public override Task<IEnumerable<string>> GetVideoSourcesAsync(string episodeUrl)
        {
            var ajaxProcessor = new AnimeOwlAjaxProcessor();
            return ajaxProcessor.GetVideoSourcesAsync(episodeUrl);
        }
        public override async Task<IEnumerable<EpisodeSource>> GetEpisodesAsync(string animeUrl)
        {
            var ajaxProcessor = new AnimeOwlAjaxProcessor();
            return await ajaxProcessor.GetEpisodesAsync(animeUrl);
        }        

        public override async Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString)
        {
            var ajaxProcessor = new AnimeOwlAjaxProcessor();
            return await ajaxProcessor.GetSearchResultsAsync(searchString);
        }

        protected override HtmlNodeCollection GetSearchItemNodes(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }
        protected override int GetSearchItemReleaseYear(HtmlNode searchItem)
        {
            var yearNode = searchItem.SelectSingleNode("/div[last()]");
            return DateTime.Parse(yearNode.InnerText.Trim()).Year;
        }
        protected override string GetSearchItemTitle(HtmlNode searchItem)
        {
            var titleNode = searchItem.SelectSingleNode(".//a/h3");
            return titleNode.InnerText.Trim();
        }
        protected override string GetSearchItemUrl(HtmlNode searchItem)
        {
            var urlNode = searchItem.SelectSingleNode(".//a");
            return urlNode.GetAttributeValue("href", String.Empty).Trim();
        }
    }
}
