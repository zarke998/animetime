using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    interface IAnimeInfoResolveExtractor
    {
        bool IsFinished { get; set; }
        string CurrentPage { get; set; }

        string RootUrl { get; set; }
        string AnimeListRootUrl { get; set; }

        IEnumerable<AnimeInfoResolve> GetAnimeInfoResolvesFromPage();
        void NextPage();

    }
}
