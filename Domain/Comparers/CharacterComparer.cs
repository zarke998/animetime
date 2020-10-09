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

            if(x.Id != 0 && y.Id != 0)
            {
                return x.Id == y.Id;
            }

            return x.SourceUrl == y.SourceUrl;
        }

        public int GetHashCode(Character c)
        {
            return c.SourceUrl.GetHashCode();
        }
    }
}
