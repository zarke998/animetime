using System;
using System.Threading.Tasks;

namespace AnimeTime.Utilities
{
    public interface ICrawlDelayer
    {
        double CrawlWait { get; set; }
        double CrawlWaitOffset { get; set; }

        void ApplyDelay(Action crawlAction);
        Task ApplyDelayAsync(Func<Task> crawlFunc);
    }
}