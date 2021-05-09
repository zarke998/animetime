namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeSourceToAnimeSourceStatusRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeSources", "Status_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AnimeSources", "Status_Id");
            AddForeignKey("dbo.AnimeSources", "Status_Id", "dbo.AnimeSourceStatuses", "Id", cascadeDelete: true);
            DropColumn("dbo.AnimeSources", "Verified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeSources", "Verified", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AnimeSources", "Status_Id", "dbo.AnimeSourceStatuses");
            DropIndex("dbo.AnimeSources", new[] { "Status_Id" });
            DropColumn("dbo.AnimeSources", "Status_Id");
        }
    }
}
