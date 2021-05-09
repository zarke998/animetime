using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace AnimeTimeDbUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using(var lifeScope = container.BeginLifetimeScope())
            {
                var app = lifeScope.Resolve<IApplication>();
                app.Run();
            }
        }
    }
}
