using System.IO;
using System.ServiceProcess;

namespace Service
{
    partial class GitMonitorService : ServiceBase
    {
        public GitMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Post the data to DB
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}