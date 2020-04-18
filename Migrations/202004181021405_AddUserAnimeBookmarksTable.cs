namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAnimeBookmarksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAnimeBookmarks",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        AnimeId = c.Int(nullable: false),
                        Notify = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.AnimeId })
                .ForeignKey("dbo.Animes", t => t.AnimeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AnimeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAnimeBookmarks", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAnimeBookmarks", "AnimeId", "dbo.Animes");
            DropIndex("dbo.UserAnimeBookmarks", new[] { "AnimeId" });
            DropIndex("dbo.UserAnimeBookmarks", new[] { "UserId" });
            DropTable("dbo.UserAnimeBookmarks");
        }
    }
}
