using SQLite;
using System;

namespace GitMonitor.DataModel
{
    [Table("Repo")]
    public class tblRepo
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public long tblRepoID { get; set; }
        public string Name { get; set; }
        public string RepoUrl { get; set; }
        public string WorkingDirectory { get; set; }
        public bool AutoTrack { get; set; }
        public bool EnableDesktopNotification { get; set; }
        public bool EnableEmailNotification { get; set; }
        public string NotificationEmail { get; set; }
        public string CurrentBranch { get; set; }
        public bool IsUntrackedRepo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? RecentCheck { get; set; }
        public bool IsActive { get; set; }
    }
}
