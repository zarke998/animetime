using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeImageConfiguration : EntityTypeConfiguration<AnimeImage>
    {
        public AnimeImageConfiguration()
        {
            HasRequired(ai => ai.Anime);
            HasRequired(ai => ai.Image).WithOptional().Map(mapConfig => mapConfig.MapKey("Image_Id"));
        }
    }
}
