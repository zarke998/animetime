using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class User
    {
        public User()
        {
            UserAnimeBookmarks = new HashSet<UserAnimeBookmark>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<UserAnimeBookmark> UserAnimeBookmarks { get; set; }
    }
}
