using System;
using System.IO;

namespace GitMonitor.UWP.Utilities
{
    internal static class LogUtility
    {
        static string _path;
        static bool _logMessage;

        static LogUtility()
        {
            //TODO need to configure in settings
            _path = "LogPath";
            _logMessage = true;
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