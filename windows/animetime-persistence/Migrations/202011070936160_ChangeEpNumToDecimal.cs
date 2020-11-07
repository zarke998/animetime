namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEpNumToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "EpNum", c => c.Decimal(nullable: false, precision: 5, scale: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "EpNum", c => c.Int(nullable: false));
        }
    }
}
