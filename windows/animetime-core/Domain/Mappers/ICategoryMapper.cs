using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain.Mappers
{
    public interface ICategoryMapper
    {
        CategoryId Map(string categoryName);
    }
}
