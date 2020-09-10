using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater
{
    public static class Constants
    {
        public const double CrawlWait = 1.0;
        public const double CrawlWaitOffset = 0.2;

        public static class WebsiteUrls
        {
            public static readonly string AnimePlanet = "https://www.anime-planet.com";
        }
    }
}
