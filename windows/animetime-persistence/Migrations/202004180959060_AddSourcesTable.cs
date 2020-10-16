namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourcesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sources",
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
            DropForeignKey("dbo.Sources", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.Sources", "EpisodeId", "dbo.Episodes");
            DropIndex("dbo.Sources", new[] { "WebsiteId" });
            DropIndex("dbo.Sources", new[] { "EpisodeId" });
            DropTable("dbo.Sources");
        }
    }
}
