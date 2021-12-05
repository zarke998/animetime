using AnimeTime.Utilities.Core.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Imaging
{
    public class ImageResizer : IImageResizer
    {
        public void Resize(Image<Rgba32> image, int maxWidthLandscape, int maxHeightPortrait)
        {
            int width = image.Width;
            int height = image.Height;
            
            int newWidth = 0;
            int newHeight = 0;

            if(height > width)
            {
                newHeight = maxHeightPortrait;
                newWidth = (newHeight * width) / height;
            }
            else
            {
                newWidth = maxWidthLandscape;
                newHeight = (newWidth * height) / width;
            }

            image.Mutate(context => context.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));
        }
    }
}
