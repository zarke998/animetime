using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class EpisodeSource
    {
        public EpisodeSource()
        {
            VideoSources = new HashSet<EpisodeVideoSource>();
        }

        public int Id { get; set; }
        public string Url { get; set; }

        public int EpisodeId { get; set; }
        public int WebsiteId { get; set; }
        public AnimeVersionIds? AnimeVersionId { get; set; }
        public Episode Episode { get; set; }
        public Website Website { get; set; }
        public AnimeVersion AnimeVersion { get; set; }

        public ICollection<EpisodeVideoSource> VideoSources { get; set; }
    }
}
