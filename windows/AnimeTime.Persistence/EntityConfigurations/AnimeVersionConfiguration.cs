using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeVersionConfiguration : EntityTypeConfiguration<AnimeVersion>
    {
        public AnimeVersionConfiguration()
        {
            Property(e => e.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(e => e.VersionName).IsRequired();
        }
    }
}
