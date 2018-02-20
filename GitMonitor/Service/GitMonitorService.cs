using System.IO;
using System.ServiceProcess;

namespace Service
{
    partial class GitMonitorService : ServiceBase
    {
        private FileSystemWatcher _watcher;

        public GitMonitorService()
        {
            InitializeComponent();

            _watcher = new FileSystemWatcher();
            _watcher.Filter = "*.git";
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;
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