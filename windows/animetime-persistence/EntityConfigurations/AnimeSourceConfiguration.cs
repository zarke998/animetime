using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeSourceConfiguration : EntityTypeConfiguration<AnimeSource>
    {
        public AnimeSourceConfiguration()
        {
            HasKey(e => new { e.AnimeId, e.WebsiteId });

            // Relationship to Anime
            HasRequired(e => e.Anime).WithMany(a => a.AnimeSources).HasForeignKey(e => e.AnimeId).WillCascadeOnDelete(true);

            // Relationship to Website
            HasRequired(e => e.Website).WithMany(w => w.AnimeSources).HasForeignKey(e => e.WebsiteId).WillCascadeOnDelete(true);

            Property(e => e.Url).IsRequired();

            HasRequired(e => e.Status).WithMany(s => s.Sources).HasForeignKey(e => e.Status_Id).WillCascadeOnDelete(true);
        }
    }
}
