using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Utilities;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    public interface IAnimeInfoExtractor
    {
        string LoadedPage { get; }
        ICrawlDelayer CrawlDelayer { get; set; }

        IEnumerable<AnimeBasicInfo> GetFromPage(string page);
        string NextPage();
    }
}
