namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeMetadatasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        EpisodesLastUpdate = c.DateTime(precision: 0, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Animes", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeMetadatas", "Id", "dbo.Animes");
            DropIndex("dbo.AnimeMetadatas", new[] { "Id" });
            DropTable("dbo.AnimeMetadatas");
        }
    }
}
