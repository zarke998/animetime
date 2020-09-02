using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class ImageLodLevel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int MaxSizeInKb { get; set; }
    }
}
