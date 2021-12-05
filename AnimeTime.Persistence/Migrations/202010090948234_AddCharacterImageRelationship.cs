namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterImageRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Image_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Image_Id");
            AddForeignKey("dbo.Characters", "Image_Id", "dbo.Images", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Image_Id", "dbo.Images");
            DropIndex("dbo.Characters", new[] { "Image_Id" });
            DropColumn("dbo.Characters", "Image_Id");
        }
    }
}
