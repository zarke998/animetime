using AnimeTime.Services.DTO;
using System.Collections.Generic;

namespace AnimeTime.Services.ModelServices.Interfaces
{
    public interface IAnimeSourceService
    {
        IEnumerable<AnimeSourceDTO> GetAll(int animeId);
    }
}