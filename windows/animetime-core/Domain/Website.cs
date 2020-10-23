using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Website
    {
        public Website()
        {
            AnimeSources = new HashSet<AnimeSource>();
            EpisodeSources = new HashSet<EpisodeSource>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<AnimeSource> AnimeSources { get; set; }
        public ICollection<EpisodeSource> EpisodeSources { get; set; }
    }
}
