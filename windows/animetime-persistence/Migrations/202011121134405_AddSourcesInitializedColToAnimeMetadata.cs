namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourcesInitializedColToAnimeMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeMetadatas", "SourcesInitialized", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeMetadatas", "SourcesInitialized");
        }
    }
}
