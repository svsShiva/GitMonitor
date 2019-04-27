using System.ServiceProcess;

namespace GitMonitor.Service
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
                //new GitMonitorService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
