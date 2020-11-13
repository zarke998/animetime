namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePKToSingleColInAnimeSources : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AnimeSources");
            AddColumn("dbo.AnimeSources", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AnimeSources", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AnimeSources");
            DropColumn("dbo.AnimeSources", "Id");
            AddPrimaryKey("dbo.AnimeSources", new[] { "AnimeId", "WebsiteId" });
        }
    }
}
