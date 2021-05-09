namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeSourceToAnimeVersionRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeSources", "AnimeVersion_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AnimeSources", "AnimeVersion_Id");
            AddForeignKey("dbo.AnimeSources", "AnimeVersion_Id", "dbo.AnimeVersions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeSources", "AnimeVersion_Id", "dbo.AnimeVersions");
            DropIndex("dbo.AnimeSources", new[] { "AnimeVersion_Id" });
            DropColumn("dbo.AnimeSources", "AnimeVersion_Id");
        }
    }
}
