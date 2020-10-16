using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Source
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int EpisodeId { get; set; }
        public int WebsiteId { get; set; }
        public Episode Episode { get; set; }
        public Website Website { get; set; }
    }
}
