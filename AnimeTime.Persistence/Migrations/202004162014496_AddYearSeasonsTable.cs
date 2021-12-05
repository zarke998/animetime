namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYearSeasonsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YearSeasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Animes", "YearSeason_Id", c => c.Int());
            CreateIndex("dbo.Animes", "YearSeason_Id");
            AddForeignKey("dbo.Animes", "YearSeason_Id", "dbo.YearSeasons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Animes", "YearSeason_Id", "dbo.YearSeasons");
            DropIndex("dbo.Animes", new[] { "YearSeason_Id" });
            DropColumn("dbo.Animes", "YearSeason_Id");
            DropTable("dbo.YearSeasons");
        }
    }
}
