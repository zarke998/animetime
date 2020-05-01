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
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnimeBookmark> UserAnimeBookmarks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Character> Characters { get; set; }


        public AnimeTimeDbContext() : base(StringConstants.AnimeTimeConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new WebsiteAnimeUrlConfiguration());
            modelBuilder.Configurations.Add(new EpisodeConfiguration());
            modelBuilder.Configurations.Add(new SourceConfiguration());
            modelBuilder.Configurations.Add(new UserAnimeBookmarkConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());
            modelBuilder.Configurations.Add(new CharacterConfiguration());           

            base.OnModelCreating(modelBuilder);
        }
    }
}
