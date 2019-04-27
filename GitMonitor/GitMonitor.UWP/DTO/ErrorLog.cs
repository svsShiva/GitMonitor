using System;

namespace GitMonitor.UWP.DTO
{
    public class ErrorLog
    {
        public long ErrorLogID { get; set; }
        public string Description { get; set; }
        public DateTime LogTime { get; set; }
    }
}