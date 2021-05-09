namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSingleRowConstraintToMetadataTable : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER MetadataSingleRowConstraintInsert
                ON dbo.Metadata
                AFTER INSERT
                AS
                BEGIN
	                SET NOCOUNT OFF;

	                DECLARE @rowCount AS INTEGER;

	                SELECT @rowCount = COUNT(*) FROM Metadata;

	                IF(@rowCount != 1)
	                BEGIN
		                RAISERROR('This table is a single-row table. Cannot insert more than one record.',16, 1);
		                ROLLBACK TRANSACTION;
	                END
                END");

            Sql(@"CREATE TRIGGER MetadataSingleRowConstraintDelete
                ON dbo.Metadata
                AFTER DELETE
                AS
                BEGIN
	                SET NOCOUNT OFF;

	                DECLARE @rowCount AS INTEGER;

	                SELECT @rowCount = COUNT(*) FROM Metadata;

	                IF(@rowCount != 1)
	                BEGIN
		                RAISERROR('This table is a single-row table. Deleting its single record is not allowed.',16, 1);
		                ROLLBACK TRANSACTION;
	                END
                END");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER IF EXISTS MetadataSingleRowConstraintDelete");
            Sql("DROP TRIGGER IF EXISTS MetadataSingleRowConstraintInsert");
        }
    }
}
