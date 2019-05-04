using SQLite;

namespace GitMonitor.DataModel
{
    [Table("Branch")]
    public class tblBranch
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public long tblBranchID { get; set; }
        public string Name { get; set; }
        public long tblRepoID { get; set; }
        public bool EnableDesktopNotification { get; set; }
        public bool EnableEmailNotification { get; set; }
        public bool AutoPull { get; set; }
        public bool IsActive { get; set; }
        public bool HasUpstream { get; set; }
        public string Remote { get; set; }
        public string TrackingBranch { get; set; }
        public long AheadBy { get; set; }
        public long BehindBy { get; set; }
    }
}
