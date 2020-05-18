namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLastInsertIdFunction : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[LastInsertId](@tableName varchar(50))
                RETURNS int
                AS
                BEGIN

                    DECLARE @id AS int;
                    SET @id = (SELECT IDENT_CURRENT(@tableName));

                    RETURN @id;
                END
                GO");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION dbo.LastInsertId;");
        }
    }
}
