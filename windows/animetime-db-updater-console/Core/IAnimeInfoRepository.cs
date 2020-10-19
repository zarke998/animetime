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

        void Resolve(AnimeInfo animeInfo);
        void ResolveRange(IEnumerable<AnimeInfo> animeInfos);

        IEnumerable<AnimeInfo> GetAll();
        IEnumerable<AnimeInfo> GetByDate();

        string NextPage();
        void ResetToFirstPage();
    }
}