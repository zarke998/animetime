namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAnimePlanetAnimeMetadatasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimePlanetAnimeMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        OrderOfInsert = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.OrderOfInsert, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AnimePlanetAnimeMetadatas", new[] { "OrderOfInsert" });
            DropTable("dbo.AnimePlanetAnimeMetadatas");
        }
    }
}
