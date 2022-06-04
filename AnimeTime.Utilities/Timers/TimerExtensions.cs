using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AnimeTime.Utilities.Timers
{
    public static class TimerExtensions
    {
        public static void Restart(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
