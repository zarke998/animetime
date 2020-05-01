using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.Repositories
{
    public class YearSeasonRepository : Repository<YearSeason>, IYearSeasonRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public YearSeasonRepository(AnimeTimeDbContext context) : base(context)
        {
        }
    }
}
