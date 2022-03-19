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

        public Episode Get(int epId, bool includeMetadata = false)
        {
            if (includeMetadata)
                return AnimeTimeDbContext.Episodes.Include(e => e.Metadata).FirstOrDefault(e => e.Id == epId);
            else
                return Get(epId);
        }
        public IEnumerable<Episode> GetWithSources(int animeId)
        {
            return AnimeTimeDbContext.Episodes
                .Include(e => e.Sources)
                .Where(e => e.AnimeId == animeId)
                .ToList();
        }
        public EpisodeMetadata GetMetadata(int epId)
        {
            return AnimeTimeDbContext.Episodes.Where(e => e.Id == epId).Select(e => e.Metadata).FirstOrDefault();
        }

        public Episode GetWithVideoSources(int episodeId)
        {
            return AnimeTimeDbContext.Episodes
                .Include(e => e.Sources.Select(source => source.VideoSources))
                .FirstOrDefault(e => e.Id == episodeId);
        }
    }
}
