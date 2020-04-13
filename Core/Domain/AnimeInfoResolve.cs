using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Persistence;

namespace AnimeTimeDbUpdater.Core.Domain
{
    public class AnimeInfoResolve
    {
        public Anime Anime { get; set; } = new Anime();
        public string AnimeDetailsUrl { get; set; }

        public string AnimeCoverThumbUrl { get; set; }
    }
}
