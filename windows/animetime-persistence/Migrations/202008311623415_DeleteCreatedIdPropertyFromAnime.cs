namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCreatedIdPropertyFromAnime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Animes", "CreatedId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Animes", "CreatedId", c => c.Int());
        }
    }
}
