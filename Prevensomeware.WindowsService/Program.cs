using System;
using System.ServiceProcess;

namespace Prevensomeware.WindowsService
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
                new PrevensomewareScheduler()
            };
            if (!Environment.UserInteractive)
                ServiceBase.Run(ServicesToRun);
            else
            {
                string[] args = {"10", ".html:.wwhtml", @"C:\Users\thewh\Desktop\Schule\7_10\Meine Webseite" };
                new PrevensomewareScheduler().DebugService(args);
            }
        }
    }
}
