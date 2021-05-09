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
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public AnimeRepository(AnimeTimeDbContext context) : base(context)
        {
        }

        public IEnumerable<string> GetAllTitles()
        {
            return AnimeTimeDbContext.Animes.Select(a => a.Title).ToList();
        }
        public Anime GetWithSources(int id, bool includeWebsites)
        {
            if (includeWebsites)
            {
                return AnimeTimeDbContext.Animes.Include(a => a.AnimeSources.Select(s => s.Website)).FirstOrDefault(a => a.Id == id);
            }
            else
            {
                return AnimeTimeDbContext.Animes.Include(a => a.AnimeSources).FirstOrDefault(a => a.Id == id);
            }
        }
        public IEnumerable<int> GetIdsWithNoSources()
        {
            return AnimeTimeDbContext.Animes.Where(a => a.AnimeSources.Count == 0).Select(a => a.Id).ToList();
        }
        public Anime GetWithAltTitles(int id)
        {
            return AnimeTimeDbContext.Animes.Include(a => a.AltTitles).FirstOrDefault(a => a.Id == id);
        }
    }
}
