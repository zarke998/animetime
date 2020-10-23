namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWebsiteAnimeUrlsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WebsiteAnimeUrls", "AnimeId", "dbo.Animes");
            DropForeignKey("dbo.WebsiteAnimeUrls", "WebsiteId", "dbo.Websites");
            DropIndex("dbo.WebsiteAnimeUrls", new[] { "AnimeId" });
            DropIndex("dbo.WebsiteAnimeUrls", new[] { "WebsiteId" });
            DropTable("dbo.WebsiteAnimeUrls");
        }
        
        public override void Down()
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
                .PrimaryKey(t => new { t.AnimeId, t.WebsiteId });
            
            CreateIndex("dbo.WebsiteAnimeUrls", "WebsiteId");
            CreateIndex("dbo.WebsiteAnimeUrls", "AnimeId");
            AddForeignKey("dbo.WebsiteAnimeUrls", "WebsiteId", "dbo.Websites", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WebsiteAnimeUrls", "AnimeId", "dbo.Animes", "Id", cascadeDelete: true);
        }
    }
}
