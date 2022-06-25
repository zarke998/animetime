using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core;
using AnimeTime.Core.Domain;
using Autofac;

namespace AnimeTimeDbUpdater
{
    class Program
    {
        private static IUnitOfWork _unitOfWork;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += AnimeTimeDbUpdater_UnhandledException;

            var container = ContainerConfig.Configure();

            using(var lifeScope = container.BeginLifetimeScope())
            {
                _unitOfWork = lifeScope.Resolve<IUnitOfWork>();                                

                var app = lifeScope.Resolve<IApplication>();
                app.Run();
            }
        }

        private static void AnimeTimeDbUpdater_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            _unitOfWork.ExceptionLogRepository.Add(new ExceptionLog()
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                DateTime = DateTime.UtcNow
            });

            _unitOfWork.Complete();
        }
    }
}
