using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Services.DTO
{
    public class AnimeLongDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public int? ReleaseYear { get; set; }
        public int? FinishYear { get; set; }
        public string Status { get; set; }
        public string YearSeason { get; set; }
        public string Category { get; set; }
        public ICollection<string> AltTitles { get; set; }
        public ICollection<ImageDTO> Images { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<CharacterDTO> Characters { get; set; }
    }
}
