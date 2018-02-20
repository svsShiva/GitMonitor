using System.Collections.Generic;

namespace DataModel
{
    public class tblRepo
    {
        public tblRepo()
        {
            this.tblBranches = new HashSet<tblBranch>();
        }

        public long tblRepoID { get; set; }
        public string Name { get; set; }
        public string RepoUrl { get; set; }
        public string WorkingDirectory { get; set; }
        public bool AutoTrack { get; set; }
        public bool EnableDesktopNotification { get; set; }
        public string NotificationEmail { get; set; }
        public long CurrentBranch { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public System.DateTime RecentCheck { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<tblBranch> tblBranches { get; set; }
        public virtual tblBranch tblBranch { get; set; }
    }
}
