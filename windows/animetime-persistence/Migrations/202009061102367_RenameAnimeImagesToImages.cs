namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAnimeImagesToImages : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AnimeImages", newName: "Images");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Images", newName: "AnimeImages");
        }
    }
}
