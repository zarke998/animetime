namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnimeImagesAnimeIdColumnIsRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes");
            DropIndex("dbo.AnimeImages", new[] { "Anime_Id" });
            AlterColumn("dbo.AnimeImages", "Anime_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AnimeImages", "Anime_Id");
            AddForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes");
            DropIndex("dbo.AnimeImages", new[] { "Anime_Id" });
            AlterColumn("dbo.AnimeImages", "Anime_Id", c => c.Int());
            CreateIndex("dbo.AnimeImages", "Anime_Id");
            AddForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes", "Id");
        }
    }
}
