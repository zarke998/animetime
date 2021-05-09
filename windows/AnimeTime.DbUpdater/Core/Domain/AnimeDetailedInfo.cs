using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Core.Domain
{
    public class AnimeDetailedInfo
    {
        public AnimeBasicInfo BasicInfo { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public int? ReleaseYear { get; set; }
        public string YearSeason { get; set; }
        public string Category { get; set; }
        public ICollection<string> AltTitles { get; set; } = new List<string>();
        public ICollection<string> Genres { get; set; } = new List<string>();

        public string CharactersUrl { get; set; }
        public string CoverUrl { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Title: {0}\n", BasicInfo.Title);
            builder.AppendFormat("Description: {0}\n", Description);
            builder.AppendFormat("Category: {0}\n", Category);
            builder.AppendFormat("Release year: {0}\n", ReleaseYear);
            builder.AppendFormat("Year season: {0}\n", YearSeason);
            builder.AppendFormat("Rating: {0}\n", Rating);

            builder.AppendLine("Alt titles:");
            foreach(var altTitle in AltTitles)
            {
                builder.AppendFormat("\t{0}\n", altTitle);
            }

            builder.AppendLine("Genres:");
            foreach (var genre in Genres)
            {
                builder.AppendFormat("\t{0}\n", genre);
            }

            builder.AppendFormat("Details url: {0}\n", BasicInfo.DetailsUrl);
            builder.AppendFormat("Cover url: {0}\n", CoverUrl);
            builder.AppendFormat("Characters url: {0}\n", CharactersUrl);

            return builder.ToString();
        }
    }
}
