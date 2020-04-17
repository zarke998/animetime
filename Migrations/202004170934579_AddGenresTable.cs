namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenresTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenreAnimes",
                c => new
                    {
                        Genre_Id = c.Int(nullable: false),
                        Anime_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_Id, t.Anime_Id })
                .ForeignKey("dbo.Genres", t => t.Genre_Id, cascadeDelete: true)
                .ForeignKey("dbo.Animes", t => t.Anime_Id, cascadeDelete: true)
                .Index(t => t.Genre_Id)
                .Index(t => t.Anime_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GenreAnimes", "Anime_Id", "dbo.Animes");
            DropForeignKey("dbo.GenreAnimes", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.GenreAnimes", new[] { "Anime_Id" });
            DropIndex("dbo.GenreAnimes", new[] { "Genre_Id" });
            DropTable("dbo.GenreAnimes");
            DropTable("dbo.Genres");
        }
    }
}
