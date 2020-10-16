using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class WebsiteAnimeUrlConfiguration : EntityTypeConfiguration<WebsiteAnimeUrl>
    {
        public WebsiteAnimeUrlConfiguration()
        {
            HasKey(e => new { e.AnimeId, e.WebsiteId });

            // Relationship to Anime
            HasRequired(e => e.Anime).WithMany(a => a.WebsiteAnimeUrls).HasForeignKey(e => e.AnimeId).WillCascadeOnDelete(true);

            // Relationship to Website
            HasRequired(e => e.Website).WithMany(w => w.WebsiteAnimeUrls).HasForeignKey(e => e.WebsiteId).WillCascadeOnDelete(true);

            Property(e => e.Url).IsRequired();
        }
    }
}
