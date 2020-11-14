using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeVersion
    {
        public AnimeVersion()
        {
            AnimeSources = new HashSet<AnimeSource>();
        }
        public AnimeVersionIds Id { get; set; }
        public string VersionName { get; set; }

        public ICollection<AnimeSource> AnimeSources { get; set; }
    }
}
