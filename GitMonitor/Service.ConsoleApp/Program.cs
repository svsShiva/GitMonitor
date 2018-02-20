using Service.ConsoleApp.Utilities;
using System;
using System.Globalization;
using System.Threading;

namespace Service.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                culture.DateTimeFormat.ShortTimePattern = "hh:mm:ss";
                culture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd";
                culture.DateTimeFormat.LongTimePattern = "hh:mm:ss";
                culture.DateTimeFormat.FullDateTimePattern = "yyyy/MM/dd hh:mm:ss";
                culture.DateTimeFormat.DateSeparator = "/";

                CultureInfo.DefaultThreadCurrentCulture = culture;

                MonitoringProcess monitoringProcess = new MonitoringProcess();
                monitoringProcess.StartProcess();

                while (true)
                {
                    Thread.Sleep(TimeSpan.FromHours(1));
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }
    }
}