using GitMonitor.UWP.DTO;
using System.Collections.Generic;

namespace GitMonitor.UWP.ViewModels
{
    public class EditFormViewModel
    {
        public EditFormViewModel()
        {
            Branches = new HashSet<Branch>();
            BranchesToNotify = new HashSet<Branch>();
        }

        public long RepoID { get; set; }
        public string RepoName { get; set; }
        public bool AutoTrack { get; set; }
        public string RepoURL { get; set; }
        public string WorkingDirectory { get; set; }
        public ICollection<Branch> Branches { get; set; }
        public ICollection<Branch> BranchesToNotify { get; set; }
        public bool EnableDesktopNotification { get; set; }
    }
}