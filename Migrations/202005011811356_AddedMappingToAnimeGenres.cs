namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedMappingToAnimeGenres : DbMigration
    {
        // Migration used to correct entity framework ManyToMany incorrect table mapping for AnimeGenres (mapping set in EntityConfigurations)
        // Notice: Manually creating migration with RenameTable commands DOES NOT change entity framework mapping for that table (Use FluentAPI and ToTable method for renaming table)
        //         Entity framework throws an error, since conceptual model is changed via EntityConfigurations but no migrations were run to update the database, 
        //         hence the reason for this empty migration
        public override void Up()
        {

        }
        public override void Down()
        {
        }
    }
}