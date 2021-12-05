using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeStatusConfiguration : EntityTypeConfiguration<AnimeStatus>
    {
        public AnimeStatusConfiguration()
        {
            ToTable("AnimeStatuses");
            
            Property(e => e.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(e => e.Name).IsRequired();
        }
    }
}
