using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class EpisodeMetadata
    {
        public int Id { get; set; }
        public DateTime? VideoSourcesLastUpdate { get; set; }
        public bool HasWorkingVideoSources { get; set; }

        public Episode Episode { get; set; }
    }
}
