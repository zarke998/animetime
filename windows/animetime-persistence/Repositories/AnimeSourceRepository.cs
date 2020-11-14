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
    public class AnimeSourceRepository : Repository<AnimeSource>, IAnimeSourceRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public AnimeSourceRepository(AnimeTimeDbContext context) : base(context)
        {
        }

        public ICollection<string> GetAllUrls()
        {
            return AnimeTimeDbContext.AnimeSources.Where(e => e.Url != null).Select(e => e.Url).ToHashSet();
        }
    }
}
