using AnimeTime.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.ModelServices.Interfaces
{
    public interface IVideoSourceService
    {
        IEnumerable<VideoSourceDTO> GetVideoSources(int episodeId);
    }
}
