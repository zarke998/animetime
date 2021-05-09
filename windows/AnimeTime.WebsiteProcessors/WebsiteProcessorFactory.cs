using AnimeTime.Core;
using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeTime.WebsiteProcessors
{
    public static class WebsiteProcessorFactory
    {
        public static IWebsiteProcessor CreateWebsiteProcessor(string websiteName, string websiteUrl, string querySuffix)
        {
            switch (websiteName.ToLower())
            {
                case "gogoanime": return new GogoanimeWebsiteProcessor(websiteUrl, querySuffix);
                default: return null;
            }
        }
    }
}