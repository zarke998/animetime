using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeMetadataConfiguration : EntityTypeConfiguration<AnimeMetadata>
    {
        public AnimeMetadataConfiguration()
        {
            Property(e => e.EpisodesLastUpdate).HasColumnType("datetime2").HasPrecision(0);
        }
    }
}
