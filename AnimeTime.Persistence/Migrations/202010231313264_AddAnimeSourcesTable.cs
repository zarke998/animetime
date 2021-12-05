namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeSourcesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeSources",
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
            DropForeignKey("dbo.AnimeSources", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.AnimeSources", "AnimeId", "dbo.Animes");
            DropIndex("dbo.AnimeSources", new[] { "WebsiteId" });
            DropIndex("dbo.AnimeSources", new[] { "AnimeId" });
            DropTable("dbo.AnimeSources");
        }
    }
}
