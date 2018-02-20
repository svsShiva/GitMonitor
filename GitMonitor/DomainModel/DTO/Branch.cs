using System.Collections.Generic;

namespace DomainModel.DTO
{
    public partial class Branch
    {
        public Branch()
        {
            this.Repos = new HashSet<Repo>();
        }

        public long BranchID { get; set; }
        public string Name { get; set; }
        public long RepoID { get; set; }
        public bool EnableDeskTopNotifications { get; set; }
        public bool AutoPull { get; set; }
        public bool IsActive { get; set; }

        public virtual Repo Repo { get; set; }
        public virtual ICollection<Repo> Repos { get; set; }
    }
}
