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
    public class ImageTypeConfiguration : EntityTypeConfiguration<ImageType>
    {
        public ImageTypeConfiguration()
        {
            Property(imageType => imageType.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(imageType => imageType.TypeName).IsRequired();
        }
    }
}
