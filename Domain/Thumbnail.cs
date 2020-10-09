using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Thumbnail
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int ImageLodLevel_Id { get; set; }
        public ImageLodLevel ImageLodLevel { get; set; }

        public Image Image { get; set; }
    }
}
