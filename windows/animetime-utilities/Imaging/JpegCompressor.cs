using AnimeTime.Utilities.Core.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimeTime.Utilities.Imaging
{
    public class JpegCompressor : IJpegCompressor
    {
        private const int compressorOriginalQuality = 85;

        public Image<Rgba32> Compress(Image<Rgba32> image, int quality)
        {
            Image<Rgba32> compressedImage;
            
            using (var stream = new MemoryStream())
            {
                JpegEncoder encoder = new JpegEncoder();
                encoder.Quality = GetConvertedQuality(quality);

                image.Save(stream, encoder);
                compressedImage = Image.Load<Rgba32>(stream.ToArray());
            }

            return compressedImage;
        }
        public async Task<Image<Rgba32>> CompressAsync(Image<Rgba32> image, int quality)
        {
            Image<Rgba32> compressedImage;

            using (var stream = new MemoryStream())
            {
                JpegEncoder encoder = new JpegEncoder();
                encoder.Quality = GetConvertedQuality(quality);

                await image.SaveAsync(stream, encoder);
                compressedImage = Image.Load(stream.ToArray());
            }

            return compressedImage;
        }

        private int GetConvertedQuality(int quality)
        {
            float percentage = quality / 100F;
            return Convert.ToInt32(percentage * compressorOriginalQuality);
        }
    }
}