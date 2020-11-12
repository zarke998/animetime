using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeMetadata
    {
        public int Id { get; set; }
        public DateTime? EpisodesLastUpdate { get; set; }
        public bool SourcesInitialized { get; set; }

        public Anime Anime { get; set; }
    }
}
