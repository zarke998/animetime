﻿using AnimeTime.Utilities;
using AnimeTimeDbUpdater.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Core
{
    interface ICharacterInfoRepository
    {
        ICrawlDelayer CrawlDelayer { get; set; }

        IEnumerable<CharacterBasicInfo> Extract(string animeCharactersUrl);
        CharacterDetailedInfo Resolve(CharacterBasicInfo characterInfo);
    }
}
