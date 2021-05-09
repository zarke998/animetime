using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            HasKey(e => new { e.UserId, e.EpisodeId });

            Property(e => e.AnimeTitle).IsRequired();
            Property(e => e.AnimeCoverUrl).IsRequired();

            HasRequired(e => e.User).WithMany(u => u.Notifications).HasForeignKey(e => e.UserId).WillCascadeOnDelete(true);
        }
    }
}
