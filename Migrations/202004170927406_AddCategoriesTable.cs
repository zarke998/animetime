namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Animes", "Category_Id", c => c.Int());
            CreateIndex("dbo.Animes", "Category_Id");
            AddForeignKey("dbo.Animes", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Animes", new[] { "Category_Id" });
            DropColumn("dbo.Animes", "Category_Id");
            DropTable("dbo.Categories");
        }
    }
}
