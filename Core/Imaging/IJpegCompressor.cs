using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IJpegCompressor
    {
        Image<Rgba32> Compress(Image<Rgba32> image, int quality); 
    }
}
