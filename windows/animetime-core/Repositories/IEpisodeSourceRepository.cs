using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Repositories
{
    public interface IEpisodeSourceRepository : IRepository<EpisodeSource>
    {
        IEnumerable<EpisodeSource> GetByEpisode(int epId, bool includeVideoSources = false, bool includeWebsites = false);
    }
}
