namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddThumbnailsTable : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Images", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropForeignKey("dbo.Images", "FK_dbo.AnimeImages_dbo.ImageLodLevels_ImageLodLevel_Id"); // FK name was based on old table name
            DropIndex("dbo.Images", new[] { "ImageLodLevel_Id" });
            CreateTable(
                "dbo.Thumbnails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Url = c.String(nullable: false),
                    Image_Id = c.Int(nullable: false),
                    ImageLodLevel_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.Image_Id, cascadeDelete: true)
                .ForeignKey("dbo.ImageLodLevels", t => t.ImageLodLevel_Id, cascadeDelete: true)
                .Index(t => t.Image_Id)
                .Index(t => t.ImageLodLevel_Id);

            AlterColumn("dbo.Images", "Url", c => c.String());
            DropColumn("dbo.Images", "ImageLodLevel_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Images", "ImageLodLevel_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropForeignKey("dbo.Thumbnails", "Image_Id", "dbo.Images");
            DropIndex("dbo.Thumbnails", new[] { "ImageLodLevel_Id" });
            DropIndex("dbo.Thumbnails", new[] { "Image_Id" });
            AlterColumn("dbo.Images", "Url", c => c.String(nullable: false));
            DropTable("dbo.Thumbnails");
            CreateIndex("dbo.Images", "ImageLodLevel_Id");
            AddForeignKey("dbo.Images", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id", name: "FK_dbo.AnimeImages_dbo.ImageLodLevels_ImageLodLevel_Id", cascadeDelete: true); // Updated according to Up method
        }
    }
}
