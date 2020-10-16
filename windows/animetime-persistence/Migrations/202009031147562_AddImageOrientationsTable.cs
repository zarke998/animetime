namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageOrientationsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageOrientations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AnimeImages", "Orientation_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AnimeImages", "Orientation_Id");
            AddForeignKey("dbo.AnimeImages", "Orientation_Id", "dbo.ImageOrientations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeImages", "Orientation_Id", "dbo.ImageOrientations");
            DropIndex("dbo.AnimeImages", new[] { "Orientation_Id" });
            DropColumn("dbo.AnimeImages", "Orientation_Id");
            DropTable("dbo.ImageOrientations");
        }
    }
}
