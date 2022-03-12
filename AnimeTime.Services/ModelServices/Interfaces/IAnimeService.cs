using AnimeTime.Services.DTO;
using System.Collections.Generic;

namespace AnimeTime.Services.ModelServices.Interfaces
{
    public interface IAnimeService
    {
        IEnumerable<AnimeSearchDTO> Search(string searchString);
    }
}