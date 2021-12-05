using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class ImageConfiguration : EntityTypeConfiguration<Image>
    {
        public ImageConfiguration()
        {
            HasRequired(i => i.ImageType).WithMany().HasForeignKey(i => i.ImageType_Id);
            HasRequired(i => i.Orientation).WithMany().HasForeignKey(i => i.Orientation_Id);
        }
    }
}
