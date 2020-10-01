using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Core
{
    public interface IImageStorage
    {
        IEnumerable<string> Upload(IEnumerable<Image<Rgba32>> images);

        Task<string> UploadAsync(Image<Rgba32> image);
        Task<IEnumerable<string>> UploadAsync(IEnumerable<Image<Rgba32>> images);
    }
}
