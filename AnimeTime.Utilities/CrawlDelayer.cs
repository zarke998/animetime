using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AnimeTime.Utilities
{
    public class CrawlDelayer : ICrawlDelayer
    {
        private const int MIN_CRAWL_DELAY_MS = 1000;
        private const int MAX_CRAWL_DELAY_MS = 4000;

        private Stopwatch _stopwatch = new Stopwatch();
        private Stopwatch _crawlStopwatch = new Stopwatch();
        private bool IsFirstCrawl { get; set; } = true;

        private double LastCrawledFor { get; set; }
        private double ElapsedTimeFromLastCrawl
        {
            get
            {
                var elapsed = _stopwatch.ElapsedMilliseconds / 1000.0;
                return elapsed;
            }
        }

        public double CrawlWait { get; set; }
        public double CrawlWaitOffset { get; set; }

        public CrawlDelayer(double crawlWait = 1.0, double crawlWaitOffset = 0.2)
        {
            CrawlWait = crawlWait;
            CrawlWaitOffset = crawlWaitOffset;
        }

        public void ApplyDelay(Action crawlAction)
        {
            if (crawlAction == null)
                return;

            if (!IsFirstCrawl)
            {
                var elapsed = ElapsedTimeFromLastCrawl;
                var timeToWait = (GetRandomCrawlDelayTime() / 1000.0) - elapsed;
                var lastCrawledFor = LastCrawledFor;

                if (timeToWait > 0)
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(timeToWait * 1000));
                }
            }
            ExecuteCrawl(crawlAction);

            IsFirstCrawl = false;
        }
        public async Task ApplyDelayAsync(Func<Task> crawlFunc)
        {
            if (crawlFunc == null)
                return;

            if (!IsFirstCrawl)
            {
                var elapsed = ElapsedTimeFromLastCrawl;
                var timeToWait = CrawlWait + CrawlWaitOffset - (elapsed);

                var lastCrawledFor = LastCrawledFor;

                if (timeToWait > 0)
                {
                    await Task.Delay((int)(timeToWait * 1000));
                }
            }
            await ExecuteCrawl(crawlFunc);

            IsFirstCrawl = false;
        }

        private double GetRandomCrawlDelayTime()
        {
            return new Random().Next(MIN_CRAWL_DELAY_MS, MAX_CRAWL_DELAY_MS);
        }

        private void ExecuteCrawl(Action crawlAction)
        {
            BeginCrawlTracking();
            crawlAction.Invoke();
            EndCrawlTracking();
        }
        private async Task ExecuteCrawl(Func<Task> crawlFunc)
        {
            BeginCrawlTracking();
            await crawlFunc();
            EndCrawlTracking();
        }
        private void BeginCrawlTracking()
        {
            _crawlStopwatch.Restart();
        }
        private void EndCrawlTracking()
        {
            _crawlStopwatch.Stop();
            LastCrawledFor = _crawlStopwatch.ElapsedMilliseconds / 1000.0;

            _stopwatch.Restart();
        }
    }
}