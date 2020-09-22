using AnimeTime.Utilities.Core.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Imaging
{
    public class ImageDownloader : IImageDownloader
    {
        public Image Download(string imageUrl)
        {
            using (WebClient client = new WebClient())
            {
                byte[] imgData = client.DownloadData(imageUrl);

                using(var memoryStream = new MemoryStream(imgData))
                {
                    var image = Image.FromStream(memoryStream);

                    return image;
                }
            }
        }
    }
}
