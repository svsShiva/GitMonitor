using System;

namespace GitMonitor.DomainModel.ViewModels
{
    public class RepoViewModel
    {
        public long RepoID { get; set; }
        public string Name { get; set; }
        public string CurrentBranch { get; set; }
        public string BranchNames { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? RecentCheck { get; set; }

        public bool AutoTrack { get; set; }

        public bool IsUntrackedRepo { get; set; }

        public string WorkingDirectory { get; set; }
    }
}