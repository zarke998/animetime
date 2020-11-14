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
        public AnimeVersionIds Id { get; set; }

        public string VersionName { get; set; }
    }
}
