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
using Thumbnail = AnimeTime.Utilities.Core.Domain.Thumbnail;

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
            var lodLevelsSorted = lodLevels.OrderBy(lod => lod.Level);
            var originalThumbnail = new Thumbnail() { Image = image };
            var originalThumbnailFound = false;

            ICollection<Thumbnail> thumbnails = new List<Thumbnail>();

            var isPortrait = image.Height > image.Width;
            foreach (var lod in lodLevelsSorted)
            {
                if((isPortrait && image.Height < lod.MaxHeightPortrait) || (!isPortrait && image.Width < lod.MaxWidthLandscape))
                {
                    originalThumbnail.LodLevel = lod.Level;

                    originalThumbnailFound = true;

                    continue;
                }

                var copy = image.Clone();

                _resizer.Resize(copy, lod.MaxWidthLandscape, lod.MaxHeightPortrait);

                var quality = Convert.ToInt32(lod.Quality * 100);
                var compressedImage = _compressor.Compress(copy, quality);

                var thumbnail = new Thumbnail() { Image = compressedImage, LodLevel = lod.Level };

                thumbnails.Add(thumbnail);
            }

            if (originalThumbnailFound)
            {
                return thumbnails.Prepend(originalThumbnail);
            }
            else
            {
                return thumbnails;
            }
        }
        public async Task<IEnumerable<Thumbnail>> GenerateAsync(Image<Rgba32> image, IEnumerable<ImageLodLevel> lodLevels)
        {
            var lodLevelsSorted = lodLevels.OrderBy(lod => lod.Level);
            var originalThumbnail = new Thumbnail() { Image = image };
            var originalThumbnailFound = false;

            ICollection<Task<Thumbnail>> thumbnailTasks = new List<Task<Thumbnail>>();
            foreach (var lod in lodLevelsSorted)
            {
                // If image is smaller than lod, add original image as thumbnail
                var isPortrait = image.Height > image.Width;
                if ((isPortrait && image.Height < lod.MaxHeightPortrait) || (!isPortrait && image.Width < lod.MaxWidthLandscape))
                {
                    originalThumbnail.LodLevel = lod.Level;

                    originalThumbnailFound = true;
                    continue;
                }

                thumbnailTasks.Add(GenerateSingleThumbnail(image, lod));
            }

            var thumbnails = await Task.WhenAll(thumbnailTasks).ConfigureAwait(false);
            if (originalThumbnailFound)
            {
                return thumbnails.Prepend(originalThumbnail);
            }
            else
            {
                return thumbnails;
            }
        }

        private async Task<Thumbnail> GenerateSingleThumbnail(Image<Rgba32> image, ImageLodLevel lodLevel)
        {
            var copy = image.Clone();

            await Task.Run(() => { _resizer.Resize(copy, lodLevel.MaxWidthLandscape, lodLevel.MaxHeightPortrait); }).ConfigureAwait(false);

            var quality = Convert.ToInt32(lodLevel.Quality * 100);
            var compressedImage = await _compressor.CompressAsync(copy, quality).ConfigureAwait(false);

            var thumbnail = new Thumbnail() { Image = compressedImage, LodLevel = lodLevel.Level };

            return thumbnail;
        }
    }
}
