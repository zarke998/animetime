﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class EpisodeVideoSource
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int EpisodeSource_Id { get; set; }
        public EpisodeSource EpisodeSource { get; set; }
    }
}
