namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedIdColumnToAnimesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Animes", "CreatedId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Animes", "CreatedId");
        }
    }
}
