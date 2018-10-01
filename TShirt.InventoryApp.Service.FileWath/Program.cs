using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TShirt.InventoryApp.Service.FileWath
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService()
            };
            ServiceBase.Run(ServicesToRun);

            //if (Environment.UserInteractive)
            //{
            //    RunInteractive(ServicesToRun);
            //}
            //else
            //{
            //    ServiceBase.Run(ServicesToRun);
            //}

        }


        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            //log4net.LogManager.GetLogger("root").Info("Start");
            
            FileWatcherService service = new FileWatcherService();
           // service.watcher_Created();

            //log4net.LogManager.GetLogger("root").Info("Finished");
            //Thread.Sleep(1000);
        }

    }
}
