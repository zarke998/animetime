using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class CharacterRoleConfiguration : EntityTypeConfiguration<CharacterRole>
    {
        public CharacterRoleConfiguration()
        {
            Property(cr => cr.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(cr => cr.RoleName).IsRequired();
        }
    }
}
