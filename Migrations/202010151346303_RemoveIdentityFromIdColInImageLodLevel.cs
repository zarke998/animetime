namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIdentityFromIdColInImageLodLevel : DbMigration
    {
        public override void Up()
        {
            RemoveIdentityFromIdCol();

            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropPrimaryKey("dbo.ImageLodLevels");
            AlterColumn("dbo.ImageLodLevels", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ImageLodLevels", "Id");
            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropPrimaryKey("dbo.ImageLodLevels");
            AlterColumn("dbo.ImageLodLevels", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ImageLodLevels", "Id");
            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id", cascadeDelete: true);

            AddIdentityToIdCol();
        }

        private void RemoveIdentityFromIdCol()
        {
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");

            CreateTable(
                "dbo.ImageLodLevels_Copy",
                c => new
                {
                    Id = c.Int(nullable: false, identity: false),
                    Level = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    MaxSizeInKb = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.ImageLodLevels_Copy", "MaxHeightPortrait", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels_Copy", "MaxWidthLandscape", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels_Copy", "Quality", c => c.Single(nullable: false));
            DropColumn("dbo.ImageLodLevels_Copy", "MaxSizeInKb");

            Sql("ALTER TABLE dbo.ImageLodLevels SWITCH TO dbo.ImageLodLevels_Copy");

            DropTable("dbo.ImageLodLevels");
            RenameTable("ImageLodLevels_Copy", "ImageLodLevels");
            Sql("EXEC sp_rename N'[dbo].[ImageLodLevels].[PK_dbo.ImageLodLevels_Copy]', 'PK_dbo.ImageLodLevels'");

            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
        }
        private void AddIdentityToIdCol()
        {
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");

            CreateTable(
                "dbo.ImageLodLevels_Copy",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Level = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    MaxSizeInKb = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.ImageLodLevels_Copy", "MaxHeightPortrait", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels_Copy", "MaxWidthLandscape", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels_Copy", "Quality", c => c.Single(nullable: false));
            DropColumn("dbo.ImageLodLevels_Copy", "MaxSizeInKb");

            Sql("ALTER TABLE dbo.ImageLodLevels SWITCH TO dbo.ImageLodLevels_Copy");

            DropTable("dbo.ImageLodLevels");
            RenameTable("ImageLodLevels_Copy", "ImageLodLevels");
            Sql("EXEC sp_rename N'[dbo].[ImageLodLevels].[PK_dbo.ImageLodLevels_Copy]', 'PK_dbo.ImageLodLevels'");

            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id");
        }
    }
}
