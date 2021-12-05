using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeAltTitleConfiguration : EntityTypeConfiguration<AnimeAltTitle>
    {
        public AnimeAltTitleConfiguration()
        {
            Property(alt => alt.Title).IsRequired();
        }
    }
}
