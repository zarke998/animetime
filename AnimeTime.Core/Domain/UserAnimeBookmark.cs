using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class UserAnimeBookmark
    {
        public int UserId { get; set; }
        public int AnimeId { get; set; }
        public bool Notify { get; set; }
        public Anime Anime { get; set; }
        public User User { get; set; }
    }
}
