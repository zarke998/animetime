using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AnimeTimeDbUpdater.Utilities
{
    public static class CrawlStopwatch
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        private static Stopwatch _crawlStopwatch = new Stopwatch();
        public static bool IsRunning { 
            get
            {
                return _stopwatch.IsRunning;
            } 
        }
        private static double LastCrawledFor { get; set; }
        private static double ElapsedTimeFromLastCrawl
        {
            get
            {
                var elapsed = _stopwatch.ElapsedMilliseconds / 1000.0;
                return elapsed;
            }            
        }
        public static void Start()
        {
            _stopwatch.Start();
        }
        public static void Restart()
        {
            _stopwatch.Restart();
        }

        public static void ApplyDelay()
        {
            if (!_stopwatch.IsRunning)
                return;

            var elapsed = CrawlStopwatch.ElapsedTimeFromLastCrawl;
            var lastCrawledFor = CrawlStopwatch.LastCrawledFor;
            var timeToWait = Constants.CrawlTime + Constants.CrawlTimeOffset - (elapsed - lastCrawledFor);

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
        public static void BeginCrawlTracking()
        {
            _stopwatch.Restart();
            _crawlStopwatch.Restart();
        }
        public static void EndCrawlTracking()
        {
            _crawlStopwatch.Stop();
            LastCrawledFor = _crawlStopwatch.ElapsedMilliseconds / 1000.0;
        }
    }
}
