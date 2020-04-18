using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Episode
    {
        public Episode()
        {
            Sources = new HashSet<Source>();
        }
        public int Id { get; set; }
        public int EpNum { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
        public ICollection<Source> Sources { get; set; }
    }
}
