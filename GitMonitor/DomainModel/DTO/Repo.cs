using System;
using System.Collections.Generic;

namespace GitMonitor.DomainModel.DTO
{
    public class Repo
    {
        public Repo()
        {
            this.Branches = new HashSet<Branch>();
        }

        public long RepoID { get; set; }
        public string Name { get; set; }
        public string RepoUrl { get; set; }
        public string WorkingDirectory { get; set; }
        public bool AutoTrack { get; set; }
        public bool EnableDesktopNotification { get; set; }
        public string NotificationEmail { get; set; }
        public string CurrentBranch { get; set; }
        public bool IsUntrackedRepo { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public Nullable<System.DateTime> RecentCheck { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
    }
}
