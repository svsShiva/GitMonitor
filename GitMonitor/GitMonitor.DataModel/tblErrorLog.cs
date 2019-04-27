using SQLite;
using System;

namespace GitMonitor.DataModel
{
    [Table("Error")]
    public class tblErrorLog
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public long tblErrorLogID { get; set; }
        public string Description { get; set; }
        public DateTime LogTime { get; set; }
    }
}
