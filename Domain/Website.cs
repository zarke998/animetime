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
            WebsiteAnimeUrls = new HashSet<WebsiteAnimeUrl>();
            Sources = new HashSet<Source>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<WebsiteAnimeUrl> WebsiteAnimeUrls { get; set; }
        public ICollection<Source> Sources { get; set; }
    }
}
