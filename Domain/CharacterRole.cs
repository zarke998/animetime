using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class CharacterRole
    {
        public CharacterRole()
        {
            Characters = new HashSet<Character>();
        }

        public CharacterRoleId Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}
