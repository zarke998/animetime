using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class WebsiteConfiguration : EntityTypeConfiguration<Website>
    {
        public WebsiteConfiguration()
        {
            Property(w => w.QuerySuffix).IsRequired();
        }
    }
}
