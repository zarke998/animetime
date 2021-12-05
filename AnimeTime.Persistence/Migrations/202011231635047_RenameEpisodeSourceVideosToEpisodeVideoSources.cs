namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEpisodeSourceVideosToEpisodeVideoSources : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EpisodeSourceVideos", newName: "EpisodeVideoSources");
            DropForeignKey("dbo.EpisodeVideoSources", "FK_dbo.EpisodeSourceVideos_dbo.EpisodeSources_EpisodeSource_Id");
            DropIndex("dbo.EpisodeVideoSources", new[] { "EpisodeSource_Id" });
            CreateIndex("dbo.EpisodeVideoSources", new[] { "EpisodeSource_Id" });
            AddForeignKey("dbo.EpisodeVideoSources", "EpisodeSource_Id", "dbo.EpisodeSources");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.EpisodeVideoSources", newName: "EpisodeSourceVideos");
            DropForeignKey("dbo.EpisodeSourceVideos", "FK_dbo.EpisodeVideoSources_dbo.EpisodeSources_EpisodeSource_Id");
            DropIndex("dbo.EpisodeSourceVideos", new[] { "EpisodeSource_Id" });
            CreateIndex("dbo.EpisodeSourceVideos", new[] { "EpisodeSource_Id" });
            AddForeignKey("dbo.EpisodeSourceVideos", "EpisodeSource_Id", "dbo.EpisodeSources");
        }
    }
}
