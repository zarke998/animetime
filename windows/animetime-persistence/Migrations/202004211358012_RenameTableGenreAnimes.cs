namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTableGenreAnimes : DbMigration
    {
        public override void Up()
        {
            RenameTable("GenreAnimes", "AnimeGenres");
        }
        
        public override void Down()
        {
            RenameTable("AnimeGenres", "GenreAnimes");
        }
    }
}
