using System;

namespace GitMonitor.DomainModel.DTO
{
    public class EmailGroup
    {
        public long EmailGroupID { get; set; }
        public string Name { get; set; }
        public string Emails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}