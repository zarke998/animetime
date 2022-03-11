using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors.Models
{
    public class AnimeSearchParams
    {
        public string Name { get; set; }
        public IEnumerable<string> AltTitles { get; set; }
        public int? ReleaseYear { get; set; }

        public AnimeSearchParams()
        {

        }
        public AnimeSearchParams(string name, IEnumerable<string> altTitles, int? releaseYear)
        {
            Name = name;
            AltTitles = altTitles;
            ReleaseYear = releaseYear;
        }
    }
}
