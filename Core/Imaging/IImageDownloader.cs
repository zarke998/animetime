using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IImageDownloader
    {
        Image<Rgba32> Download(string imageUrl);
    }
}
