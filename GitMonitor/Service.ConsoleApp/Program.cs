using GitMonitor.Service.ConsoleApp.Utilities;
using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.SelfHost;

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

                FileMonitorUtility fileSystemWatcher = new FileMonitorUtility();

                TimerUtility monitoringProcess = new TimerUtility();
                monitoringProcess.StartTimer();

                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:8002");

                config.Routes.MapHttpRoute(
                    name: "GitMonitorAPI",
                    routeTemplate: "{Api}/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional });

                using (HttpSelfHostServer server = new HttpSelfHostServer(config))
                {
                    server.OpenAsync().Wait();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }
    }
}