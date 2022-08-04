using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Image
    {
        public Image()
        {
            Thumbnails = new HashSet<Thumbnail>();
            AnimeImages = new HashSet<AnimeImage>();
        }

        public int Id { get; set; }
        public string Url { get; set; }

        public ImageTypeId ImageType_Id { get; set; }
        public ImageType ImageType { get; set; }

        public ImageOrientationId Orientation_Id { get; set; }
        public ImageOrientation Orientation { get; set; }

        public ICollection<Thumbnail> Thumbnails { get; set; }
        public ICollection<AnimeImage> AnimeImages { get; set; }
    }
}
