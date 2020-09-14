using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Comparers
{
    public class CharacterComparer : IEqualityComparer<Character>
    {
        public bool Equals(Character x, Character y)
        {
            if (y == null)
                return false;

            return x.SourceUrl == y.SourceUrl;
        }

        public int GetHashCode(Character c)
        {
            return c.SourceUrl.GetHashCode();
        }
    }
}
