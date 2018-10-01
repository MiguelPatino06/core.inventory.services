using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TShirt.InventoryApp.Services.FileWatcher
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


        //private static void RunInteractive(ServiceBase[] servicesToRun)
        //{  
        //       FileWatcherService service = new FileWatcherService();                   
        //}
    }
}
