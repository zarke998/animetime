namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasWorkingVideoSorucesToEpisodeMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EpisodeMetadatas", "HasWorkingVideoSources", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EpisodeMetadatas", "HasWorkingVideoSources");
        }
    }
}
