using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AnimeTime.Core.Utilities.Imaging
{
    public interface IImageDownloader
    {
        Image Download(string imageUrl);
    }
}
