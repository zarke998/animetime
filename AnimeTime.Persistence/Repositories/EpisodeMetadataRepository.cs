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
    public class EpisodeMetadataRepository : Repository<EpisodeMetadata>, IEpisodeMetadataRepository 
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public EpisodeMetadataRepository(AnimeTimeDbContext context) : base(context)
        {

        }
    }
}
