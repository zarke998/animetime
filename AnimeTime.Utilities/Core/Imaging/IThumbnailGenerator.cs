using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTime.Utilities.Core.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Thumbnail = AnimeTime.Utilities.Core.Domain.Thumbnail;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IThumbnailGenerator
    {
        IEnumerable<Thumbnail> Generate(Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels);
        Task<IEnumerable<Thumbnail>> GenerateAsync(Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels);
    }
}