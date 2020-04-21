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
            Notifications = new HashSet<Notification>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<UserAnimeBookmark> UserAnimeBookmarks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
