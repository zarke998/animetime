namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToImageLodLevelsAndDeleteSome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageLodLevels", "MaxHeightPortrait", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels", "MaxWidthLandscape", c => c.Int(nullable: false));
            AddColumn("dbo.ImageLodLevels", "Quality", c => c.Single(nullable: false));
            DropColumn("dbo.ImageLodLevels", "MaxSizeInKb");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImageLodLevels", "MaxSizeInKb", c => c.Int(nullable: false));
            DropColumn("dbo.ImageLodLevels", "Quality");
            DropColumn("dbo.ImageLodLevels", "MaxWidthLandscape");
            DropColumn("dbo.ImageLodLevels", "MaxHeightPortrait");
        }
    }
}
