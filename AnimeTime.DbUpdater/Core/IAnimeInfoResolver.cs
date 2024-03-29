﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTime.Utilities;
using AnimeTimeDbUpdater.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    public interface IAnimeInfoResolver
    {
        ICrawlDelayer CrawlDelayer { get; set; }

        AnimeDetailedInfo Resolve(AnimeBasicInfo basicInfo);
    }
}
