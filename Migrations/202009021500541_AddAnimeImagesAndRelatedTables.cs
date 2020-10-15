namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeImagesAndRelatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        Anime_Id = c.Int(nullable: false),
                        ImageLodLevel_Id = c.Int(nullable: false),
                        ImageType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Animes", t => t.Anime_Id, cascadeDelete: true)
                .ForeignKey("dbo.ImageLodLevels", t => t.ImageLodLevel_Id, cascadeDelete: true)
                .ForeignKey("dbo.ImageTypes", t => t.ImageType_Id, cascadeDelete: true)
                .Index(t => t.Anime_Id)
                .Index(t => t.ImageLodLevel_Id)
                .Index(t => t.ImageType_Id);
            
            CreateTable(
                "dbo.ImageLodLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Level = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        MaxSizeInKb = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeImages", "ImageType_Id", "dbo.ImageTypes");
            DropForeignKey("dbo.AnimeImages", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropForeignKey("dbo.AnimeImages", "Anime_Id", "dbo.Animes");
            DropIndex("dbo.AnimeImages", new[] { "ImageType_Id" });
            DropIndex("dbo.AnimeImages", new[] { "ImageLodLevel_Id" });
            DropIndex("dbo.AnimeImages", new[] { "Anime_Id" });
            DropTable("dbo.ImageTypes");
            DropTable("dbo.ImageLodLevels");
            DropTable("dbo.AnimeImages");
        }
    }
}
