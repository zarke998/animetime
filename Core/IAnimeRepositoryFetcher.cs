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
        IEnumerable<Anime> ResolveRange(IEnumerable<AnimeInfoResolve> animeInfoResolves);
        Anime Resolve(AnimeInfoResolve animeInfoResolve);
        IEnumerable<AnimeInfoResolve> GetAnimeInfoResolves();
    }
}
