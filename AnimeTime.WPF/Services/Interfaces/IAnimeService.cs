using AnimeTime.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services.Interfaces
{
    public interface IAnimeService
    {
        Task<AnimeDTO> GetAnimeShort(int id);
        Task<IEnumerable<EpisodeDTO>> GetEpisodesAsync(int animeId);
    }
}
