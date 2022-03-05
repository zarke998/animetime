using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimePlanetAnimeMetadataConfiguration : EntityTypeConfiguration<AnimePlanetAnimeMetadata>
    {
        public AnimePlanetAnimeMetadataConfiguration()
        {
            HasIndex(e => e.OrderOfInsert).IsUnique();
        }
    }
}
