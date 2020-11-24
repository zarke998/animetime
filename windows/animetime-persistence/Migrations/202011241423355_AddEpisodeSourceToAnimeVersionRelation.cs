namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeSourceToAnimeVersionRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EpisodeSources", "AnimeVersionId", c => c.Int());
            CreateIndex("dbo.EpisodeSources", "AnimeVersionId");
            AddForeignKey("dbo.EpisodeSources", "AnimeVersionId", "dbo.AnimeVersions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EpisodeSources", "AnimeVersionId", "dbo.AnimeVersions");
            DropIndex("dbo.EpisodeSources", new[] { "AnimeVersionId" });
            DropColumn("dbo.EpisodeSources", "AnimeVersionId");
        }
    }
}
