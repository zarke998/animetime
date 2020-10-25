using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeSourceStatusesConfiguration : EntityTypeConfiguration<AnimeSourceStatus>
    {
        public AnimeSourceStatusesConfiguration()
        {
            ToTable("AnimeSourceStatuses");

            Property(e => e.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(e => e.Name).IsRequired();
        }
    }
}