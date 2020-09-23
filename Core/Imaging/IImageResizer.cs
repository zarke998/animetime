using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IImageResizer
    {
        void Resize(Image<Rgba32> image, int maxWidthLandscape, int maxHeightPortrait);
    }
}
