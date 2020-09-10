using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeAltTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Anime Anime { get; set; }
    }
}
