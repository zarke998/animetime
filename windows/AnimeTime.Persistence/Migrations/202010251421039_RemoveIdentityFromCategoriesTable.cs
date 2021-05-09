namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RemoveIdentityFromCategoriesTable : DbMigration
    {
        public override void Up()
        {
            RemoveIdentity();

            // Generated
            DropForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Categories", "Id");
            AddForeignKey("dbo.Animes", "Category_Id", "dbo.Categories", "Id");
        }

        private void RemoveIdentity()
        {
            DropForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");

            CreateTable(
                "dbo.Categories_Copy",
                cb => new
                {
                    Id = cb.Int(nullable: false, identity: false),
                    Name = cb.String()
                })
                .PrimaryKey(c => c.Id);

            Sql("ALTER TABLE dbo.Categories SWITCH TO dbo.Categories_Copy");

            DropTable("dbo.Categories");
            RenameTable("Categories_Copy", "Categories");

            DropPrimaryKey("dbo.Categories", "PK_dbo.Categories_Copy");
            AddPrimaryKey("dbo.Categories", "Id");

            AddForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");
        }
        private void AddIdentity()
        {
            DropForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");

            CreateTable(
                "dbo.Categories_Copy",
                cb => new
                {
                    Id = cb.Int(nullable: false, identity: true),
                    Name = cb.String()
                })
                .PrimaryKey(c => c.Id);

            Sql("ALTER TABLE dbo.Categories SWITCH TO dbo.Categories_Copy");

            DropTable("dbo.Categories");
            RenameTable("Categories_Copy", "Categories");

            DropPrimaryKey("dbo.Categories", "PK_dbo.Categories_Copy");
            AddPrimaryKey("dbo.Categories", "Id");

            AddForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");
        }

        public override void Down()
        {

            // Generated
            DropForeignKey("dbo.Animes", "Category_Id", "dbo.Categories");
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Categories", "Id");
            AddForeignKey("dbo.Animes", "Category_Id", "dbo.Categories", "Id");

            AddIdentity();
        }
    }
}
