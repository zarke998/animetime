﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTime.Core.Domain;

namespace AnimeTimeDbUpdater.Core
{
    public interface IAnimeInfoRepository
    {
        bool CanFetchByDateAdded { get; }
        string CurrentPage { get; }
        bool LastPageReached { get; }

        string AnimeListUrl { get; }
        string AnimeListByDateUrl { get; }

        AnimeDetailedInfo Resolve(AnimeBasicInfo basicInfo);
        IEnumerable<AnimeDetailedInfo> ResolveRange(IEnumerable<AnimeBasicInfo> basicInfos);

        IEnumerable<AnimeBasicInfo> GetAll();
        IEnumerable<AnimeBasicInfo> GetByDate();

        string NextPage();
        void ResetToFirstPage();
    }
}