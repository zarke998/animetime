using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class MetadataConfiguration : EntityTypeConfiguration<Metadata>
    {
        public MetadataConfiguration()
        {
            ToTable("Metadata");
        }
    }
}
