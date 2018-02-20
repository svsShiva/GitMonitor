using System.Collections.Generic;

namespace DataModel
{
    public partial class tblBranch
    {
        public tblBranch()
        {
            this.tblRepos = new HashSet<tblRepo>();
        }

        public long tblBranchID { get; set; }
        public string Name { get; set; }
        public long tblRepoID { get; set; }
        public bool EnableDeskTopNotifications { get; set; }
        public bool AutoPull { get; set; }
        public bool IsActive { get; set; }

        public virtual tblRepo tblRepo { get; set; }
        public virtual ICollection<tblRepo> tblRepos { get; set; }
    }
}
