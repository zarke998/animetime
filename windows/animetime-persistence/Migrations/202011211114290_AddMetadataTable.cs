namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMetadataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Metadata",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnimeSourcesInitialized = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AnimeMetadatas", "SourcesInitialized");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeMetadatas", "SourcesInitialized", c => c.Boolean(nullable: false));
            DropTable("dbo.Metadata");
        }
    }
}
