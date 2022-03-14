using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors.Models
{
    public class AnimeSource
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public AnimeSourceStatus Status_Id { get; set; }

        public AnimeSource()
        {

        }
        public AnimeSource(string name, string url, AnimeSourceStatus status)
        {
            Name = name;
            Url = url;
            Status_Id = status;
        }
    }

    public enum AnimeSourceStatus
    {
        CouldNotResolve,
        Resolved
    }
}
