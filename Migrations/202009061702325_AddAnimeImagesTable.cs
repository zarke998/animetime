namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeImagesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "FK_dbo.AnimeImages_dbo.Animes_Anime_Id"); // Manually added since the EF generated command throws an exception
            DropIndex("dbo.Images", new[] { "Anime_Id" });                                  // probably because the table was renamed
            CreateTable(
                "dbo.AnimeImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Anime_Id = c.Int(),
                        Image_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.Image_Id)
                .ForeignKey("dbo.Animes", t => t.Anime_Id)
                .Index(t => t.Anime_Id)
                .Index(t => t.Image_Id);
            
            DropColumn("dbo.Images", "Anime_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Anime_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes");
            DropForeignKey("dbo.AnimeImages", "Image_Id", "dbo.Images");
            DropIndex("dbo.AnimeImages", new[] { "Image_Id" });
            DropIndex("dbo.AnimeImages", new[] { "Anime_Id" });
            DropTable("dbo.AnimeImages");
            CreateIndex("dbo.Images", "Anime_Id");

            AddForeignKey("dbo.Images", "Anime_Id", "dbo.Animes", "Id", cascadeDelete: true, name: "FK_dbo.AnimeImages_dbo.Animes_Anime_Id");
        }
    }
}
