using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Core.Domain
{
    public class CharacterInfo
    {
        public Character Character { get; set; } = new Character();

        public string ImageUrl { get; set; }
    }
}
