using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Episode
    {
        public int Id { get; set; }
        public int EpNum { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
    }
}
