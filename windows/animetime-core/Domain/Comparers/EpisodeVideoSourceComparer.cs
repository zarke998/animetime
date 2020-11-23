using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Comparers
{
    public class EpisodeVideoSourceComparer : IEqualityComparer<EpisodeVideoSource>
    {
        public bool Equals(EpisodeVideoSource x, EpisodeVideoSource y)
        {
            if (y == null || x.Url == null || y.Url == null) return false;

            return x.Url == y.Url;
        }

        public int GetHashCode(EpisodeVideoSource obj)
        {
            if (obj.Url == null || obj.Url == null) return 0;

            return obj.Url.GetHashCode();
        }
    }
}
