using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Persistence.Mappers
{
    public class AnimePlanetCharacterRoleMapper : IEnumMapper<CharacterRoleId>
    {
        public CharacterRoleId Map(string value)
        {
            switch (value)
            {
                case "Main": return CharacterRoleId.Main;
                case "Secondary": return CharacterRoleId.Secondary;
                case "Minor": return CharacterRoleId.Minor;
                default: return CharacterRoleId.Other;
            }
        }
    }
}
