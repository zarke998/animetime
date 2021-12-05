using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Imaging
{
    public static class Extensions
    {
        public static async Task<Stream> ToStreamAsync(this Image<Rgba32> image)
        {
            var stream = new MemoryStream();
            await image.SaveAsync(stream, new JpegEncoder()).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        public static bool IsPortrait(this Image<Rgba32> image)
        {
            return image.Height > image.Width;
        }
    }
}
