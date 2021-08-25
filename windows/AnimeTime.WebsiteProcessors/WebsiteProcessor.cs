﻿using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public abstract class WebsiteProcessor : IWebsiteProcessor
    {
        protected string _websiteUrl;
        protected string _querySuffix;

        protected HtmlWeb _web;

        protected abstract char WhitespaceDelimiter { get; }
        protected abstract string DubAnimeIdentifier { get; }

        public ICrawlDelayer CrawlDelayer { get; set; }

        public WebsiteProcessor(string websiteUrl, string querySuffix)
        {
            this._websiteUrl = websiteUrl;
            this._querySuffix = querySuffix;

            this._web = new HtmlWeb();
        }

        public abstract Task<(string animeUrl, string animeDubUrl)> GetAnimeUrlAsync(string animeName, int? releaseYear, IEnumerable<string> animeAltTitles);
        public abstract Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString);
        public abstract Task<IEnumerable<(float epNum, string epUrl)>> GetAnimeEpisodesAsync(string animeUrl);
        public abstract Task<IEnumerable<string>> GetVideoSourcesForEpisodeAsync(string episodeUrl);
    }
}