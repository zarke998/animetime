using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Character
    {
        public Character()
        {
            Animes = new HashSet<Anime>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
        public CharacterRole Role { get; set; }

        public ICollection<Anime> Animes { get; set; }
    }
}
