namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAddEpisodeToNotificationsTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER dbo.AddEpisodeToNotifications 
                ON dbo.Episodes 
                AFTER INSERT
                AS 
                BEGIN
	                SET NOCOUNT ON;

	                INSERT INTO Notifications (UserId, EpisodeId, EpisodeNumber, AnimeTitle, AnimeCoverUrl)
	                SELECT UserId, inserted.Id, EpNum, a.Title, ISNULL(a.CoverThumbUrl, a.CoverUrl)
	                FROM inserted
	                INNER JOIN Animes AS a ON inserted.AnimeId = a.Id
	                INNER JOIN UserAnimeBookmarks uab ON uab.AnimeId = a.Id
	                WHERE Notify = 1;
                END");
        }
      
        
        public override void Down()
        {
            Sql("DROP TRIGGER IF EXISTS AddEpisodeToNotifications");
        }
    }
}
