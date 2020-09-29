using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTime.Utilities.Core.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IThumbnailGenerator
    {
        IEnumerable<Thumbnail> Generate(SixLabors.ImageSharp.Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels);
        Task<IEnumerable<Thumbnail>> GenerateAsync(SixLabors.ImageSharp.Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels);
    }
}