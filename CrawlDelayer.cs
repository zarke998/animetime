using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AnimeTime.Utilities
{
    public static class CrawlDelayer
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        private static Stopwatch _crawlStopwatch = new Stopwatch();

        private const double CrawlWait = 1.0;
        private const double CrawlWaitOffset = 0.2;

        private static bool IsFirstCrawl { get; set; } = true;

        private static double LastCrawledFor { get; set; }
        private static double ElapsedTimeFromLastCrawl
        {
            get
            {
                var elapsed = _stopwatch.ElapsedMilliseconds / 1000.0;
                return elapsed;
            }
        }

        public static void ApplyDelay(Action crawlAction)
        {
            if (crawlAction == null)
                return;

            if (!IsFirstCrawl)
            {
                var elapsed = ElapsedTimeFromLastCrawl;
                var lastCrawledFor = LastCrawledFor;
                var timeToWait = CrawlWait + CrawlWaitOffset - (elapsed - lastCrawledFor);

#if DEBUG
                Console.WriteLine($"\nLast crawled for: {lastCrawledFor}");
                Console.WriteLine($"Last crawl elapsed: {elapsed}");
                Console.WriteLine($"Time to wait: {timeToWait}");
                Console.WriteLine();
#endif
                if (timeToWait > 0)
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(timeToWait * 1000));
                }
            }
            ExecuteCrawl(crawlAction);

            IsFirstCrawl = false;
        }

        private static void ExecuteCrawl(Action crawlAction)
        {
            BeginCrawlTracking();
            crawlAction.Invoke();
            EndCrawlTracking();
        }
        private static void BeginCrawlTracking()
        {
            _stopwatch.Restart();
            _crawlStopwatch.Restart();
        }
        private static void EndCrawlTracking()
        {
            _crawlStopwatch.Stop();
            LastCrawledFor = _crawlStopwatch.ElapsedMilliseconds / 1000.0;
        }
    }
}