using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    interface IAnimeInfoExtractor
    {
        string LoadedPage { get; }

        IEnumerable<AnimeBasicInfo> GetFromPage(string page);
        string NextPage();
    }
}
