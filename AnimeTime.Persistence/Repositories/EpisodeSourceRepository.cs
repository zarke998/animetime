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

        public IEnumerable<EpisodeSource> GetByEpisode(int epId, bool includeVideoSources = false, bool includeWebsites = false)
        {
            IQueryable<EpisodeSource> query = AnimeTimeDbContext.EpisodeSources.Where(e => e.EpisodeId == epId);

            if (includeVideoSources)
                query = query.Include(e => e.VideoSources);
            if (includeWebsites)
                query = query.Include(e => e.Website);

            return query.ToList();
        }
    }
}
