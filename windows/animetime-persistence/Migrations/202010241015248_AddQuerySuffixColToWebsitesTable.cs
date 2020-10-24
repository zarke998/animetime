namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuerySuffixColToWebsitesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Websites", "QuerySuffix", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Websites", "QuerySuffix");
        }
    }
}
