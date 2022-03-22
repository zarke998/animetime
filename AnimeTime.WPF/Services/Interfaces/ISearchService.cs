using AnimeTime.Services.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<AnimeSearchDTO>> SearchAsync(string searchString);
    }
}