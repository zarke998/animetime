using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Utilities
{
    public static class Log
    {
        private static readonly TraceSource _traceSource = new TraceSource("AnimeTimeDbUpdater");

        public static TraceSource TraceSource 
        { 
            get
            {
                return _traceSource;
            }
        }

        public static void TraceEvent(TraceEventType eventType, int id, string message)
        {
            _traceSource.TraceEvent(eventType, id, message);
            _traceSource.Flush();
        }
    }
}
