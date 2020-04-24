using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class CharacterConfiguration : EntityTypeConfiguration<Character>
    {
        public CharacterConfiguration()
        {
            Property(e => e.Name).IsRequired();

            HasMany(e => e.Animes).WithMany(a => a.Characters).Map(m => m.ToTable("AnimeCharacters"));
        }
    }
}
