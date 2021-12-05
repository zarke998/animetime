namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSourcesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sources", "EpisodeId", "dbo.Episodes");
            DropForeignKey("dbo.Sources", "WebsiteId", "dbo.Websites");
            DropIndex("dbo.Sources", new[] { "EpisodeId" });
            DropIndex("dbo.Sources", new[] { "WebsiteId" });
            DropTable("dbo.Sources");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Sources", "WebsiteId");
            CreateIndex("dbo.Sources", "EpisodeId");
            AddForeignKey("dbo.Sources", "WebsiteId", "dbo.Websites", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sources", "EpisodeId", "dbo.Episodes", "Id", cascadeDelete: true);
        }
    }
}
