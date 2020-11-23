using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class EpisodeMetadataConfiguration : EntityTypeConfiguration<EpisodeMetadata>
    {
        public EpisodeMetadataConfiguration()
        {
            Property(e => e.VideoSourcesLastUpdate).HasColumnType("datetime2").HasPrecision(0); // Precision(scale) of milliseconds
        }
    }
}
