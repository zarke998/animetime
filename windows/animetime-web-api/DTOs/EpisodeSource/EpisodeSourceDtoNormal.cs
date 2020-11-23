using AnimeTime.WebAPI.DTOs.EpisodeVideoSource;
using AnimeTime.WebAPI.DTOs.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeTime.WebAPI.DTOs.EpisodeSource
{
    public class EpisodeSourceDtoNormal
    {
        public string Url { get; set; }
        public WebsiteDtoShort Website { get; set; }
        public ICollection<EpisodeVideoSourceDtoShort> VideoSources { get; set; }
    }
}