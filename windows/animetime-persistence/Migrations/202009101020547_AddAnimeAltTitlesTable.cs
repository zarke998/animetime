namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeAltTitlesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeAltTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Anime_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Animes", t => t.Anime_Id)
                .Index(t => t.Anime_Id);
            
            DropColumn("dbo.Animes", "TitleAlt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Animes", "TitleAlt", c => c.String());
            DropForeignKey("dbo.AnimeAltTitles", "Anime_Id", "dbo.Animes");
            DropIndex("dbo.AnimeAltTitles", new[] { "Anime_Id" });
            DropTable("dbo.AnimeAltTitles");
        }
    }
}
