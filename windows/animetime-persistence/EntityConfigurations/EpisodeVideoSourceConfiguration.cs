using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class EpisodeVideoSourceConfiguration : EntityTypeConfiguration<EpisodeVideoSource>
    {
        public EpisodeVideoSourceConfiguration()
        {
            Property(e => e.Url).IsRequired();
            HasRequired(e => e.EpisodeSource).WithMany(s => s.VideoSources).HasForeignKey(e => e.EpisodeSource_Id).WillCascadeOnDelete(false);
        }
    }
}
