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
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Anime List.txt");
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
