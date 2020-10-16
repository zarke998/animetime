using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class WebsiteAnimeUrl
    {
        public int WebsiteId { get; set; }
        public int AnimeId { get; set; }
        public string Url { get; set; }
        public bool Verified { get; set; }
        public Anime Anime { get; set; }
        public Website Website { get; set; }
    }
}
