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

            HasRequired(ai => ai.Anime).WithMany(a => a.Images).HasForeignKey(ai => ai.Anime_Id);
            HasRequired(ai => ai.Image).WithMany(i => i.AnimeImages).HasForeignKey(ai => ai.Image_Id).WillCascadeOnDelete(false);
        }
    }
}
