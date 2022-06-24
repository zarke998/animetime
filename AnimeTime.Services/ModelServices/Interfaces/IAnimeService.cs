using AnimeTime.Services.DTO;
using System.Collections.Generic;

namespace AnimeTime.Services.ModelServices.Interfaces
{
    public interface IAnimeService
    {
        AnimeDTO GetAnimeShort(int id);
        IEnumerable<AnimeSearchDTO> Search(string searchString);
    }
}