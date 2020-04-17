using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence
{
    public class AnimeTimeDbContext : DbContext
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<YearSeason> YearSeasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public AnimeTimeDbContext() : base(StringConstants.AnimeTimeConnectionString)
        {

        }
    }
}
