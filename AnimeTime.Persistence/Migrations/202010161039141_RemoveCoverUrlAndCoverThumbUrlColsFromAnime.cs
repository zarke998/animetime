namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCoverUrlAndCoverThumbUrlColsFromAnime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Animes", "CoverUrl");
            DropColumn("dbo.Animes", "CoverThumbUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Animes", "CoverThumbUrl", c => c.String());
            AddColumn("dbo.Animes", "CoverUrl", c => c.String());
        }
    }
}
