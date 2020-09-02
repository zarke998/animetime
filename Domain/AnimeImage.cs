using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public ImageType ImageType { get; set; }
        public ImageLodLevel ImageLodLevel { get; set; }
        public Anime Anime { get; set; }
    }
}
