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
        bool IsFinished { get; }

        void Initialize(string websiteUrl, string animeListUrl);
        IEnumerable<AnimeInfoResolve> GetAnimeInfoResolvesFromPage();
        void NextPage();

    }
}
