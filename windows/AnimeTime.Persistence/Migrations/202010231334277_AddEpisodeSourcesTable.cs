namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeSourcesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EpisodeSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        EpisodeId = c.Int(nullable: false),
                        WebsiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episodes", t => t.EpisodeId, cascadeDelete: true)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.EpisodeId)
                .Index(t => t.WebsiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EpisodeSources", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.EpisodeSources", "EpisodeId", "dbo.Episodes");
            DropIndex("dbo.EpisodeSources", new[] { "WebsiteId" });
            DropIndex("dbo.EpisodeSources", new[] { "EpisodeId" });
            DropTable("dbo.EpisodeSources");
        }
    }
}
