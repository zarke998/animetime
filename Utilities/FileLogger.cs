using AnimeTimeDbUpdater.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Utilities
{
    public class FileLogger : ILogger
    {
        private string _path;

        public FileLogger()
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            Directory.CreateDirectory(_path);

            var timestamp = DateTime.UtcNow.ToString("dd_MM_yyyy H-mm-ss");
            _path = Path.Combine(_path, $"{timestamp}.txt");

            if (File.Exists(_path))
                File.Delete(_path);            
        }

        public void Log(string message)
        {
            using (StreamWriter writer = File.AppendText(_path))
                writer.WriteLine(message);
        }
    }
}
