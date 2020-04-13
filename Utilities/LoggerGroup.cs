using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTimeDbUpdater.Core;

namespace AnimeTimeDbUpdater.Utilities
{
    public class LoggerGroup : ILoggerGroup
    {
        private ICollection<ILogger> _logGroup = new List<ILogger>();

        public LoggerGroup(FileLogger fileLogger, ConsoleLogger consoleLogger)
        {
            _logGroup.Add(fileLogger);
            _logGroup.Add(consoleLogger);
        }
        public void LogAll(string message)
        {
            foreach (var logger in _logGroup)
                logger.Log(message);
        }
    }
}
