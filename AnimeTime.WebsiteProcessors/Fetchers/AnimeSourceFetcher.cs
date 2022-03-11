using AnimeTime.WebsiteProcessors.Interfaces;
using AnimeTime.WebsiteProcessors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors.Fetchers
{
    public class AnimeSourceFetcher : IAnimeSourceFetcher
    {
        private readonly IWebsiteProcessorFactory _websiteProcessorFactory;

        public AnimeSourceFetcher(IWebsiteProcessorFactory websiteProcessorFactory)
        {
            this._websiteProcessorFactory = websiteProcessorFactory;
        }

        public IEnumerable<AnimeSourceSubDub> Fetch(AnimeSearchParams searchParams, IEnumerable<string> websiteNames)
        {
            throw new NotImplementedException();
        }
    }
}
