using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors.Models
{
    public class AnimeSourceSubDub
    {
        public AnimeSource Sub { get; set; } = new AnimeSource();
        public AnimeSource Dub { get; set; } = new AnimeSource();
        public bool IsResolved => Sub.Status == AnimeSourceStatus.Resolved || Dub.Status == AnimeSourceStatus.Resolved; 
    }
}
