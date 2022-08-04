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

        public Anime GetLongInfo(int id)
        {
            var anime = AnimeTimeDbContext.Animes
                            .Include(a => a.Status)
                            .Include(a => a.YearSeason)
                            .Include(a => a.Category)
                            .Include(a => a.Images.Select(ai => ai.Image.ImageType))
                            .Include(a => a.Images.Select(ai => ai.Image).Select(i => i.Thumbnails.Select(t => t.ImageLodLevel)))                            
                            .Include(a => a.AltTitles)
                            .Include(a => a.Genres)
                            .Include(a => a.Characters.Select(c => c.Role))
                            .Include(a => a.Characters.Select(c => c.Image).Select(i => i.Thumbnails.Select(t => t.ImageLodLevel)))
                        .FirstOrDefault(a => a.Id == id);

            return anime;
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
        public Anime GetWithAltTitles(int id)
        {
            return AnimeTimeDbContext.Animes.Include(a => a.AltTitles).FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<string> GetAllTitles()
        {
            return AnimeTimeDbContext.Animes.Select(a => a.Title).ToList();
        }
        public IEnumerable<int> GetIdsWithNoSources()
        {
            return AnimeTimeDbContext.Animes.Where(a => a.AnimeSources.Count == 0).Select(a => a.Id).ToList();
        }

        public IEnumerable<Anime> Search(string searchString)
        {
            return AnimeTimeDbContext.Animes.Include(a => a.AltTitles)
                                            .Include(a => a.Images.Select(i => i.Image))
                                            .Where(a => a.Title.ToLower().Contains(searchString.ToLower()) ||
                                                        a.AltTitles.Any(altTitle => altTitle.Title.ToLower().Contains(searchString.ToLower())))
                                            .ToList();
        }
    }
}
