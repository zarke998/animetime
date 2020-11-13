using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeSource
    {
        public int Id { get; set; }

        public int WebsiteId { get; set; }
        public int AnimeId { get; set; }
        public string Url { get; set; }

        public AnimeSourceStatusIds Status_Id { get; set; }
        public AnimeSourceStatus Status { get; set; }

        public Anime Anime { get; set; }
        public Website Website { get; set; }
    }
}
