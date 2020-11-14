namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUrlAndAnimeVersionToNullableInAnimeSources : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AnimeSources", new[] { "AnimeVersion_Id" });
            AlterColumn("dbo.AnimeSources", "Url", c => c.String());
            AlterColumn("dbo.AnimeSources", "AnimeVersion_Id", c => c.Int());
            CreateIndex("dbo.AnimeSources", "AnimeVersion_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AnimeSources", new[] { "AnimeVersion_Id" });
            AlterColumn("dbo.AnimeSources", "AnimeVersion_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AnimeSources", "Url", c => c.String(nullable: false));
            CreateIndex("dbo.AnimeSources", "AnimeVersion_Id");
        }
    }
}
