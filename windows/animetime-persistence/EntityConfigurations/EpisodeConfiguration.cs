using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class EpisodeConfiguration : EntityTypeConfiguration<Episode>
    {
        public EpisodeConfiguration()
        {
            Property(e => e.EpNum).HasPrecision(5, 1);
            HasRequired(e => e.Anime).WithMany(a => a.Episodes).HasForeignKey(e => e.AnimeId).WillCascadeOnDelete(true);
        }
    }
}
