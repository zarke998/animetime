using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Notification
    {
        public int UserId { get; set; }
        public string AnimeTitle { get; set; }
        public string AnimeCoverUrl { get; set; }
        public int EpisodeId { get; set; }
        public int EpisodeNumber { get; set; }

        public User User { get; set; }
        public Episode Episode { get; set; }

    }
}
