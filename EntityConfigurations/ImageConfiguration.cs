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
            Property(i => i.Url).IsRequired();
            HasRequired(i => i.ImageType).WithMany();
            HasRequired(i => i.ImageLodLevel).WithMany();
            HasRequired(i => i.Anime).WithMany(a => a.Images);
            HasRequired(i => i.Orientation).WithMany();
        }
    }
}
