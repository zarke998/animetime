using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Core
{
    public interface IEnumMapper<T> where T : Enum
    {
        T Map(string value);
    }
}
