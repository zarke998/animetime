namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeToAnimeStatusesRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Animes", "Status_Id", c => c.Int());
            CreateIndex("dbo.Animes", "Status_Id");
            AddForeignKey("dbo.Animes", "Status_Id", "dbo.AnimeStatuses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Animes", "Status_Id", "dbo.AnimeStatuses");
            DropIndex("dbo.Animes", new[] { "Status_Id" });
            DropColumn("dbo.Animes", "Status_Id");
        }
    }
}
