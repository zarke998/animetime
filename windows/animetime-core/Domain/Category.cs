using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Category
    {
        public Category()
        {
            Animes = new HashSet<Anime>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Anime> Animes { get; set; }
    }
}
