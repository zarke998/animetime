using AnimeTime.Core;
using AnimeTime.Persistence;
using AnimeTime.Utilities;
using System;

namespace AnimeTimeAnimeSourceUpdater
{
    internal class ClassFactory
    {
        public static IUnitOfWork CreateUnitOfWork() => new UnitOfWork(new AnimeTimeDbContext());
        public static ICrawlDelayer CreateCrawlDelayer(double crawlWait = 1.0, double crawlWaitOffset = 0.2) => new CrawlDelayer(crawlWait, crawlWaitOffset);
    }
}