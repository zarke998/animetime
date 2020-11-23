namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeSourceVideosTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EpisodeSourceVideos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        EpisodeSource_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EpisodeSources", t => t.EpisodeSource_Id)
                .Index(t => t.EpisodeSource_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EpisodeSourceVideos", "EpisodeSource_Id", "dbo.EpisodeSources");
            DropIndex("dbo.EpisodeSourceVideos", new[] { "EpisodeSource_Id" });
            DropTable("dbo.EpisodeSourceVideos");
        }
    }
}
