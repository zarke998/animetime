using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Core.Imaging
{
    public interface IImageDownloader
    {
        Image Download(string imageUrl);
    }
}
