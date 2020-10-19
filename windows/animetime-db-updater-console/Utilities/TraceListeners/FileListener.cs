using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace AnimeTimeDbUpdater.Utilities.TraceListeners
{
    public class FileListener : TextWriterTraceListener
    {
        public FileListener()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy_MM_dd HH-mm-ss");

            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

            Directory.CreateDirectory(logFolder);

            var fileStream = new StreamWriter(File.Open(Path.Combine(logFolder, $"{timestamp}.log"), FileMode.Create));            

            Writer = fileStream;
        }
        public FileListener(string logFile)
        {
            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

            Directory.CreateDirectory(logFolder);

            var fileStream = new StreamWriter(File.Open(Path.Combine(logFolder, $"{logFile}.log"), FileMode.Create));

            Writer = fileStream;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy_MM_dd HH-mm-ss");

            base.TraceEvent(eventCache, source, eventType, id, timestamp + " " + message);
        }
    }
}
