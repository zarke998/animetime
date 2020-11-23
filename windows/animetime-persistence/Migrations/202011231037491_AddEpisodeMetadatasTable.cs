namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeMetadatasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EpisodeMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        VideoSourcesLastUpdate = c.DateTime(precision: 0, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episodes", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EpisodeMetadatas", "Id", "dbo.Episodes");
            DropIndex("dbo.EpisodeMetadatas", new[] { "Id" });
            DropTable("dbo.EpisodeMetadatas");
        }
    }
}
