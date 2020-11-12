﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AnimeTime.Utilities
{
    public class CrawlDelayer : ICrawlDelayer
    {
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
                var timeToWait = CrawlWait + CrawlWaitOffset - (elapsed);

                var lastCrawledFor = LastCrawledFor;

                if (timeToWait > 0)
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(timeToWait * 1000));
                }
            }
            ExecuteCrawl(crawlAction);

            IsFirstCrawl = false;
        }

        private void ExecuteCrawl(Action crawlAction)
        {
            BeginCrawlTracking();
            crawlAction.Invoke();
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