namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Episodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EpNum = c.Int(nullable: false),
                        AnimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Animes", t => t.AnimeId, cascadeDelete: true)
                .Index(t => t.AnimeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Episodes", "AnimeId", "dbo.Animes");
            DropIndex("dbo.Episodes", new[] { "AnimeId" });
            DropTable("dbo.Episodes");
        }
    }
}
