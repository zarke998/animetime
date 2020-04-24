namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharactersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnimeCharacters",
                c => new
                    {
                        Character_Id = c.Int(nullable: false),
                        Anime_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Character_Id, t.Anime_Id })
                .ForeignKey("dbo.Characters", t => t.Character_Id, cascadeDelete: true)
                .ForeignKey("dbo.Animes", t => t.Anime_Id, cascadeDelete: true)
                .Index(t => t.Character_Id)
                .Index(t => t.Anime_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeCharacters", "Anime_Id", "dbo.Animes");
            DropForeignKey("dbo.AnimeCharacters", "Character_Id", "dbo.Characters");
            DropIndex("dbo.AnimeCharacters", new[] { "Anime_Id" });
            DropIndex("dbo.AnimeCharacters", new[] { "Character_Id" });
            DropTable("dbo.AnimeCharacters");
            DropTable("dbo.Characters");
        }
    }
}
