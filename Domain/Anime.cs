using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class Anime
    {
        public Anime()
        {
            AltTitles = new HashSet<AnimeAltTitle>();
            Genres = new HashSet<Genre>();
            WebsiteAnimeUrls = new HashSet<WebsiteAnimeUrl>();
            Episodes = new HashSet<Episode>();
            UserAnimeBookmarks = new HashSet<UserAnimeBookmark>();
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string CoverThumbUrl { get; set; }
        public float Rating { get; set; }
        public int? ReleaseYear { get; set; }
        public YearSeason YearSeason { get; set; }
        public Category Category { get; set; }
        public ICollection<AnimeAltTitle> AltTitles { get; set; }
        public ICollection<AnimeImage> Images { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<WebsiteAnimeUrl> WebsiteAnimeUrls { get; set; }
        public ICollection<Episode> Episodes { get; set; }
        public ICollection<UserAnimeBookmark> UserAnimeBookmarks { get; set; }
        public ICollection<Character> Characters { get; set; }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine($"Title: {Title}");
            s.AppendLine($"AltTitle: { (AltTitles.Count != 0 ? GetAltTitles() : "(none)") }");
            s.AppendLine($"\nDescription: {Description }\n");
            s.AppendLine($"CoverUrl: {CoverUrl}");
            s.AppendLine($"CoverThumbUrl: {CoverThumbUrl}");
            s.AppendLine($"ReleaseYear: {ReleaseYear}");
            s.AppendLine($"YearSeason: { (YearSeason != null ? YearSeason.Name : "(none)") }");
            s.AppendLine($"Rating: {Rating}");
            s.AppendLine($"Category: {Category.Name}");

            return s.ToString();
        }

        private string GetAltTitles()
        {
            string altTitles = String.Empty;

            foreach(var title in AltTitles) {
                altTitles += $",{title.Title}";
            }
            altTitles = altTitles.Substring(1);

            return altTitles;
        }
    }
}
