using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class UserAnimeBookmarkConfiguration : EntityTypeConfiguration<UserAnimeBookmark>
    {
        public UserAnimeBookmarkConfiguration()
        {
            HasKey(e => new { e.UserId, e.AnimeId });

            // Relationship to Anime
            HasRequired(e => e.Anime).WithMany(a => a.UserAnimeBookmarks).HasForeignKey(e => e.AnimeId).WillCascadeOnDelete(true);

            // Relationship to User
            HasRequired(e => e.User).WithMany(u => u.UserAnimeBookmarks).HasForeignKey(e => e.UserId).WillCascadeOnDelete(true);
        }
    }
}
