using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AnimeTime.Persistence.Repositories
{
    public class EpisodeSourceRepository : Repository<EpisodeSource>, IEpisodeSourceRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public EpisodeSourceRepository(AnimeTimeDbContext context) : base(context)
        {

        }

        public IEnumerable<EpisodeSource> GetWithVideoSources(int epId)
        {
            return AnimeTimeDbContext.EpisodeSources.Include(e => e.VideoSources).Where(e => e.EpisodeId == epId).ToList();
        }
    }
}
