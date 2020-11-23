using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Repositories
{
    public interface IEpisodeRepository : IRepository<Episode>
    {
        Episode Get(int epId, bool includeMetadata = false);
        EpisodeMetadata GetMetadata(int epId);
        IEnumerable<Episode> GetWithSources(int animeId);
    }
}
