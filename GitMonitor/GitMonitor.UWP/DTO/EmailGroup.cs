using System;

namespace GitMonitor.UWP.DTO
{
    public class tblEmailGroup
    {
        public long tblEmailGroupID { get; set; }
        public string Name { get; set; }
        public string Emails { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}