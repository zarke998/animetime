using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class SourceConfiguration : EntityTypeConfiguration<Source>
    {
        public SourceConfiguration()
        {
            Property(s => s.Url).IsRequired();

            // Relationship to Episode
            HasRequired(s => s.Episode).WithMany(e => e.Sources).HasForeignKey(s => s.EpisodeId).WillCascadeOnDelete(true);

            // Relationship to Website
            HasRequired(s => s.Website).WithMany(w => w.Sources).HasForeignKey(s => s.WebsiteId).WillCascadeOnDelete(true);
        }
    }
}
