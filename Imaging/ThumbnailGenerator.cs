using AnimeTime.Core.Domain;
using AnimeTime.Utilities.Core.Domain;
using AnimeTime.Utilities.Core.Imaging;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace AnimeTime.Utilities.Imaging
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        IImageResizer _resizer;
        IJpegCompressor _compressor;

        public ThumbnailGenerator(IImageResizer resizer, IJpegCompressor compressor)
        {
            _resizer = resizer;
            _compressor = compressor;
        }

        public IEnumerable<Thumbnail> Generate(Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels)
        {
            ICollection<Thumbnail> thumbnails = new List<Thumbnail>();

            foreach (var lod in lodLevels)
            {
                var copy = image.Clone();

                _resizer.Resize(copy, lod.MaxWidthLandscape, lod.MaxHeightPortrait);

                var quality = Convert.ToInt32(lod.Quality * 100);
                var compressedImage = _compressor.Compress(copy, quality);

                var thumbnail = new Thumbnail() { Image = compressedImage, LodLevel = lod.Level };

                thumbnails.Add(thumbnail);
            }

            return thumbnails;
        }
        public async Task<IEnumerable<Thumbnail>> GenerateAsync(Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels)
        {
            ICollection<Task<Thumbnail>> thumbnailTasks = new List<Task<Thumbnail>>();
            foreach (var lod in lodLevels)
            {
                thumbnailTasks.Add(GenerateSingleThumbnail(image, lod));
            }

            return await Task.WhenAll(thumbnailTasks);
        }

        private async Task<Thumbnail> GenerateSingleThumbnail(Image<Rgba32> image, ImageLodLevel lodLevel)
        {
            var copy = image.Clone();

            await Task.Run(() => { _resizer.Resize(copy, lodLevel.MaxWidthLandscape, lodLevel.MaxHeightPortrait); });

            var quality = Convert.ToInt32(lodLevel.Quality * 100);
            var compressedImage = await _compressor.CompressAsync(copy, quality);

            var thumbnail = new Thumbnail() { Image = compressedImage, LodLevel = lodLevel.Level };

            return thumbnail;
        }
    }
}
