using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AnimeTime.Core.Domain;
using AnimeTime.Persistence.EntityConfigurations;

namespace AnimeTime.Persistence
{
    public class AnimeTimeDbContext : DbContext
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<YearSeason> YearSeasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<WebsiteAnimeUrl> WebsiteAnimeUrls { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        public AnimeTimeDbContext() : base(StringConstants.AnimeTimeConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new WebsiteAnimeUrlConfiguration());
            modelBuilder.Configurations.Add(new EpisodeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
