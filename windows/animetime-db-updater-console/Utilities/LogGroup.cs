using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Utilities.Core.Loggers;
using AnimeTime.Utilities.Loggers;

namespace AnimeTimeDbUpdater.Utilities
{
    public static class LogGroup
    {
        private static ICollection<ILogger> _logGroup = new List<ILogger>();

        static LogGroup()
        {
            _logGroup.Add(new FileLogger());
            _logGroup.Add(new ConsoleLogger());
        }
        public static void Log(string message)
        {
            foreach (var logger in _logGroup)
                logger.Log(message);
        }
    }
}
