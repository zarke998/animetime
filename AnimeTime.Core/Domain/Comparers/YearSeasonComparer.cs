using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Comparers
{
    public class YearSeasonComparer : IEqualityComparer<YearSeason>
    {
        public bool Equals(YearSeason x, YearSeason y)
        {
            if (y == null)
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(YearSeason y)
        {
            return y.Name.GetHashCode();
        }
    }
}
