using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;

namespace AnimeTimeDbUpdater.Core.Domain
{
    public class AnimeInfoResolvable
    {
        public Anime Anime { get; set; } = new Anime();
        public string AnimeDetailsUrl { get; set; }

        public string AnimeCoverThumbUrl { get; set; }
    }
}
