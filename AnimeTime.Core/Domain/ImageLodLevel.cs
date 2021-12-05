using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class ImageLodLevel
    {
        public int Id { get; set; }
        public LodLevel Level { get; set; }
        public string Name { get; set; }
        public int MaxHeightPortrait { get; set; }
        public int MaxWidthLandscape { get; set; }
        public float Quality { get; set; }
    }
}
