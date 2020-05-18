using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Comparers
{
    public class GenreComparer : IEqualityComparer<Genre>
    {
        public bool Equals(Genre x, Genre y)
        {
            if (y == null)
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(Genre g)
        {
            return g.Name.GetHashCode();
        }
    }
}
