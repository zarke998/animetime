using AnimeTime.Core.Domain.Enums;
using System.Collections.Generic;

namespace AnimeTime.Core.Domain
{
    public class AnimeStatus
    {
        public AnimeStatus()
        {
            Animes = new HashSet<Anime>();
        }

        public AnimeStatusIds Id { get; set; }
        public string Name { get; set; }

        public ICollection<Anime> Animes { get; set; }
    }
}