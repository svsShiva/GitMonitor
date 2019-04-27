using SQLite;
using System;

namespace GitMonitor.DataModel
{
    [Table("EmailGroup")]
    public class tblEmailGroup
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public long tblEmailGroupID { get; set; }
        public string Name { get; set; }
        public string Emails { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}