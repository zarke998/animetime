using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Domain
{
    public class AnimeSourceStatus
    {
        public AnimeSourceStatus()
        {
            Sources = new HashSet<AnimeSource>();
        }

        public AnimeSourceStatusIds Id { get; set; }
        public string Name { get; set; }

        public ICollection<AnimeSource> Sources { get; set; }
    }
}
