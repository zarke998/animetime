namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFkOnEpisodeSource_IdToNotNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EpisodeVideoSources", new[] { "EpisodeSource_Id" });
            AlterColumn("dbo.EpisodeVideoSources", "EpisodeSource_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.EpisodeVideoSources", "EpisodeSource_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EpisodeVideoSources", new[] { "EpisodeSource_Id" });
            AlterColumn("dbo.EpisodeVideoSources", "EpisodeSource_Id", c => c.Int());
            CreateIndex("dbo.EpisodeVideoSources", "EpisodeSource_Id");
        }
    }
}
