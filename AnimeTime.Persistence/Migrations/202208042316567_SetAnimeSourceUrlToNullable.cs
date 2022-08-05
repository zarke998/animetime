namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetAnimeSourceUrlToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnimeSources", "Url", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AnimeSources", "Url", c => c.String(nullable: false));
        }
    }
}
