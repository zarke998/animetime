namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeReleaseYearToNullableInAnimesTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Animes", "ReleaseYear", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Animes", "ReleaseYear", c => c.Int(nullable: false));
        }
    }
}
