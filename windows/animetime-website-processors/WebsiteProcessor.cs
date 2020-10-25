using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Core.WebsiteProcessors;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public abstract class WebsiteProcessor : IWebsiteProcessor
    {
        protected string _websiteUrl;
        protected string _querySuffix;

        protected HtmlWeb _web;

        protected abstract char _whiteSpaceDelimiter { get; }
        protected abstract string _dubAnimeIdentifier { get; }

        public WebsiteProcessor(string websiteUrl, string querySuffix)
        {
            this._websiteUrl = websiteUrl;
            this._querySuffix = querySuffix;

            this._web = new HtmlWeb();
        }
        
        public abstract (string animeUrl, string animeDubUrl) GetAnimeUrl(string animeName, int releaseYear, string animeAltTitle);
        public abstract IEnumerable<(string Title, string Url, int releaseYear)> SearchAnimes(string searchString);

        public abstract IEnumerable<(int epNum, string epUrl)> GetEpisodes(string animeUrl);
    }
}