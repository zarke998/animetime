namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetAnimeSourceUrlToNonNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnimeSources", "Url", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AnimeSources", "Url", c => c.String());
        }
    }
}
