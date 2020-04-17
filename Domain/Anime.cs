﻿using System;
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
            Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string CoverThumbUrl { get; set; }
        public float Rating { get; set; }
        public int ReleaseYear { get; set; }
        public YearSeason YearSeason { get; set; }
        public Category Category { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
