using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Comparers
{
    public class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            if (y == null)
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(Category c)
        {
            return c.Name.GetHashCode();
        }
    }
}
