using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimeTime.Utilities.Core.Domain
{
    public class Thumbnail
    {
        public Image<Rgba32> Image { get; set; }
        public LodLevel LodLevel { get; set; }
    }
}
