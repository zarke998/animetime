using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class ImageOrienatationConfiguration : EntityTypeConfiguration<ImageOrientation>
    {
        public ImageOrienatationConfiguration()
        {
            Property(io => io.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(io => io.Name).IsRequired();
        }
    }
}
