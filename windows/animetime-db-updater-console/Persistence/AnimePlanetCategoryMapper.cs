using AnimeTime.Core.Domain.Enums;
using AnimeTime.Core.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Persistence
{
    public class AnimePlanetCategoryMapper : ICategoryMapper
    {
        public CategoryId Map(string categoryName)
        {
            switch (categoryName)
            {
                case "TV": return CategoryId.TV;
                case "Movie":  return CategoryId.Movie;
                case "DVD Special": return CategoryId.DVD_Special;
                case "Web": return CategoryId.Web;
                case "OVA": return CategoryId.OVA;
                default: return CategoryId.Other;
            }
        }
    }
}
