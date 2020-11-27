namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFinishYearToAnime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Animes", "FinishYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Animes", "FinishYear");
        }
    }
}
