namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEpNumTypeToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "EpNum", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "EpNum", c => c.Decimal(nullable: false, precision: 5, scale: 1));
        }
    }
}
