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

        public static bool IsRunning { 
            get
            {
                return _stopwatch.IsRunning;
            } 
        }
        public static double LastCrawledFor { get; set; }
        public static double ElapsedTimeFromLastCrawl
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
    }
}
