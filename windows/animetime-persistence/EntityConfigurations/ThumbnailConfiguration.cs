using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class ThumbnailConfiguration : EntityTypeConfiguration<Thumbnail>
    {
        public ThumbnailConfiguration()
        {
            Property(t => t.Url).IsRequired();

            HasRequired(t => t.Image).WithMany(i => i.Thumbnails);
            HasRequired(t => t.ImageLodLevel).WithMany().HasForeignKey(i => i.ImageLodLevel_Id);
        }
    }
}
