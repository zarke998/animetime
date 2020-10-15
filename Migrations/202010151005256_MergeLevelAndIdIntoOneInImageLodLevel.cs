namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeLevelAndIdIntoOneInImageLodLevel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropPrimaryKey("dbo.ImageLodLevels");
            AlterColumn("dbo.ImageLodLevels", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ImageLodLevels", "Id");
            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id", cascadeDelete: true);
            DropColumn("dbo.ImageLodLevels", "Level");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImageLodLevels", "Level", c => c.Int(nullable: false));
            DropForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels");
            DropPrimaryKey("dbo.ImageLodLevels");
            AlterColumn("dbo.ImageLodLevels", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ImageLodLevels", "Id");
            AddForeignKey("dbo.Thumbnails", "ImageLodLevel_Id", "dbo.ImageLodLevels", "Id", cascadeDelete: true);
        }
    }
}
