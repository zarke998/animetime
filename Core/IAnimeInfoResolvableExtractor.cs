using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    interface IAnimeInfoResolvableExtractor
    {
        bool IsFinished { get; }
        string AnimeListUrl { get; }
        string WebsiteUrl { get; }
        string CurrentPage { get; }
        bool IsSessionStarted { get; }

        void StartExtractSession(string animeListUrl, string websiteUrl);
        void EndExtractSession();
        IEnumerable<AnimeInfoResolvable> GetResolvablesFromPage(string page);
        string NextPage();

    }
}
