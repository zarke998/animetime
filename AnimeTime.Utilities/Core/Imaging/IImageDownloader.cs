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
        ICrawlDelayer CrawlDelayer { get; set; }

        Image<Rgba32> Download(string imageUrl);
    }
}
