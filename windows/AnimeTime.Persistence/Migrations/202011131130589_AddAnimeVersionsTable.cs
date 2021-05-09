namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeVersionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeVersions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        VersionName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnimeVersions");
        }
    }
}
