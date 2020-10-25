using AnimeTime.Core.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeTime.WebAPI.Core.WebsiteProcessors
{
    public abstract class WebsiteProcessor : IWebsiteProcessor
    {
        protected Website _website;
        protected HtmlWeb _web;

        protected abstract char _whiteSpaceDelimiter { get; }
        protected abstract string _dubAnimeIdentifier { get; }

        public WebsiteProcessor(Website website)
        {
            this._website = website;
            this._web = new HtmlWeb();
        }
        
        public abstract string GetAnimeUrl(string animeName, string animeAltTitle);

        public abstract IEnumerable<(string Title, string Url)> SearchAnimes(string searchString);
    }
}