namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionAndSourceUrlToCharacterTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Description", c => c.String());
            AddColumn("dbo.Characters", "SourceUrl", c => c.String());
            DropColumn("dbo.Characters", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "ImageUrl", c => c.String());
            DropColumn("dbo.Characters", "SourceUrl");
            DropColumn("dbo.Characters", "Description");
        }
    }
}
