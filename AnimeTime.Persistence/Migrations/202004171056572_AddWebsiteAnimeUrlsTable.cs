namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWebsiteAnimeUrlsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebsiteAnimeUrls",
                c => new
                    {
                        AnimeId = c.Int(nullable: false),
                        WebsiteId = c.Int(nullable: false),
                        Url = c.String(nullable: false),
                        Verified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnimeId, t.WebsiteId })
                .ForeignKey("dbo.Animes", t => t.AnimeId, cascadeDelete: true)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.AnimeId)
                .Index(t => t.WebsiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebsiteAnimeUrls", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.WebsiteAnimeUrls", "AnimeId", "dbo.Animes");
            DropIndex("dbo.WebsiteAnimeUrls", new[] { "WebsiteId" });
            DropIndex("dbo.WebsiteAnimeUrls", new[] { "AnimeId" });
            DropTable("dbo.WebsiteAnimeUrls");
        }
    }
}
