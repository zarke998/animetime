using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Utilities.TraceListeners
{
    public class ConsoleWithoutPrefixListener : ConsoleTraceListener
    {
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if(eventType == TraceEventType.Information || eventType == TraceEventType.Verbose)
            {
                WriteLine(message);
            }
            else
            {
                base.TraceEvent(eventCache, source, eventType, id, message);
            }
        }
    }
}
