namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnimeSourceStatusesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeSourceStatuses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnimeSourceStatuses");
        }
    }
}
