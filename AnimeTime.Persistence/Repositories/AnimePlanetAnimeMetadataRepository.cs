using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.Repositories
{
    public class AnimePlanetAnimeMetadataRepository : Repository<AnimePlanetAnimeMetadata>, IAnimePlanetAnimeMetadataRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public AnimePlanetAnimeMetadataRepository(AnimeTimeDbContext context) : base(context)
        {

        }
    }
}
