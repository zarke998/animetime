namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        EpisodeId = c.Int(nullable: false),
                        AnimeTitle = c.String(nullable: false),
                        AnimeCoverUrl = c.String(nullable: false),
                        EpisodeNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.EpisodeId })
                .ForeignKey("dbo.Episodes", t => t.EpisodeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EpisodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "EpisodeId", "dbo.Episodes");
            DropIndex("dbo.Notifications", new[] { "EpisodeId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropTable("dbo.Notifications");
        }
    }
}
