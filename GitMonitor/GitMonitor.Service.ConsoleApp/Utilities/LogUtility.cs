using System;
using System.Configuration;
using System.IO;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    static class LogUtility
    {
        static string _path = ConfigurationManager.AppSettings["LogPath"].ToString();
        static bool _logMessage = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLog"].ToString());

        static LogUtility()
        {
            _path = ConfigurationManager.AppSettings["LogPath"].ToString();
            _logMessage = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLog"].ToString());
        }

        public static void LogMessage(string message)
        {
            try
            {
                if (_logMessage)
                {
                    using (StreamWriter file = File.AppendText(_path))
                    {
                        file.WriteLine(DateTime.Now.ToString() + " " + message);
                    }
                }
            }
            catch { }
        }

        public static void LogMessage(Exception exception)
        {
            try
            {
                if (_logMessage)
                {
                    using (StreamWriter file = File.AppendText(_path))
                    {
                        file.WriteLine("{0} {1} {2} {3}",
                              DateTime.Now.ToString(),
                              exception.Message,
                              exception.InnerException != null ? exception.InnerException.Message : "",
                              exception.StackTrace);
                    }
                }
            }
            catch { }
        }
    }
}
