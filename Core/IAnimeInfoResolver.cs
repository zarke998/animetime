using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Persistence;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    public interface IAnimeInfoResolver
    {
        Anime Resolve(AnimeInfoResolve animeInfoResolve);
    }
}
