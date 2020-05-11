using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTime.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    public interface IAnimeRepositoryFetcher
    {
        bool CanFetchByDateAdded { get; }
        string CurrentPage { get; }

        string AnimeListUrl { get; }
        string AnimeListByDateAddedUrl { get; }
        string WebsiteUrl { get; }

        Anime Resolve(AnimeInfoResolvable animeInfoResolve);
        IEnumerable<Anime> ResolveRange(IEnumerable<AnimeInfoResolvable> animeInfoResolves);
        IEnumerable<AnimeInfoResolvable> GetAllAnimeInfoResolvables();
        IEnumerable<AnimeInfoResolvable> GetAnimeInfoResolvablesByDateAdded(string page);
        string NextPage();
        void ResetFetcher();
    }
}