using AnimeTime.Utilities.Core.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Imaging
{
    public class ImageDownloader : IImageDownloader
    {
        public ICrawlDelayer CrawlDelayer { get; set; }

        public Image<Rgba32> Download(string imageUrl)
        {
            using (WebClient client = new WebClient())
            {
                byte[] imgData = null;
                if (CrawlDelayer != null)
                    CrawlDelayer.ApplyDelay(() => imgData = client.DownloadData(imageUrl));
                else
                    imgData = client.DownloadData(imageUrl);

                var image = Image.Load(imgData);
                return image;
            }
        }
    }
}
