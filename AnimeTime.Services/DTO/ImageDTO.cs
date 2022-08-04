using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ImageType { get; set; }
        public ICollection<ThumbnailDTO> Thumbnails { get; set; }
    }
}
