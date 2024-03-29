﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AnimeTime.Core.Domain;
using AnimeTime.Persistence.EntityConfigurations;

namespace AnimeTime.Persistence
{
    [DbConfigurationType(typeof(AnimeTimeDbContextConfiguration))]
    public class AnimeTimeDbContext : DbContext
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<YearSeason> YearSeasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnimeBookmark> UserAnimeBookmarks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterRole> CharacterRoles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<ImageType> ImageTypes { get; set; }
        public DbSet<ImageLodLevel> ImageLodLevels { get; set; }
        public DbSet<ImageOrientation> ImageOrientations { get; set; }
        public DbSet<AnimeImage> AnimeImages { get; set; }
        public DbSet<AnimeAltTitle> AnimeAltTitles { get; set; }
        public DbSet<AnimeSource> AnimeSources { get; set; }
        public DbSet<EpisodeSource> EpisodeSources { get; set; }
        public DbSet<AnimeSourceStatus> AnimeSourceStatuses { get; set; }
        public DbSet<AnimeMetadata> AnimeMetadatas { get; set; }
        public DbSet<AnimeVersion> AnimeVersions { get; set; }
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<EpisodeMetadata> EpisodeMetadatas { get; set; }
        public DbSet<EpisodeVideoSource> EpisodeVideoSources { get; set; }
        public DbSet<AnimeStatus> AnimeStatuses { get; set; }
        public DbSet<AnimePlanetAnimeMetadata> AnimePlanetAnimeMetadatas { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public AnimeTimeDbContext() : base(StringConstants.AnimeTimeConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EpisodeConfiguration());
            modelBuilder.Configurations.Add(new UserAnimeBookmarkConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());
            modelBuilder.Configurations.Add(new CharacterConfiguration());
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new AnimeConfiguration());
            modelBuilder.Configurations.Add(new ImageConfiguration());
            modelBuilder.Configurations.Add(new ImageTypeConfiguration());
            modelBuilder.Configurations.Add(new ImageLodLevelConfiguration());
            modelBuilder.Configurations.Add(new ImageOrienatationConfiguration());
            modelBuilder.Configurations.Add(new AnimeImageConfiguration());
            modelBuilder.Configurations.Add(new CharacterRoleConfiguration());
            modelBuilder.Configurations.Add(new AnimeAltTitleConfiguration());
            modelBuilder.Configurations.Add(new ThumbnailConfiguration());
            modelBuilder.Configurations.Add(new AnimeSourceConfiguration());
            modelBuilder.Configurations.Add(new EpisodeSourceConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new AnimeSourceStatusesConfiguration());
            modelBuilder.Configurations.Add(new AnimeMetadataConfiguration());
            modelBuilder.Configurations.Add(new AnimeVersionConfiguration());
            modelBuilder.Configurations.Add(new MetadataConfiguration());
            modelBuilder.Configurations.Add(new EpisodeMetadataConfiguration());
            modelBuilder.Configurations.Add(new EpisodeVideoSourceConfiguration());
            modelBuilder.Configurations.Add(new AnimeStatusConfiguration());
            modelBuilder.Configurations.Add(new AnimePlanetAnimeMetadataConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
