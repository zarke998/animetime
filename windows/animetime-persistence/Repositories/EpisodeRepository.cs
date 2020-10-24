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
    public class EpisodeRepository : Repository<Episode> , IEpisodeRepository 
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public EpisodeRepository(AnimeTimeDbContext context) : base(context)
        {

        }

        public IEnumerable<Episode> GetWithSources(int animeId)
        {
            return AnimeTimeDbContext.Episodes
                .Include(e => e.Sources)
                .Where(e => e.AnimeId == animeId)
                .ToList();
        }
    }
}
