using System;

namespace AnimeTime.Utilities
{
    public interface ICrawlDelayer
    {
        double CrawlWait { get; set; }
        double CrawlWaitOffset { get; set; }

        void ApplyDelay(Action crawlAction);
    }
}