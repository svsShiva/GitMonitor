using DataModel;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel.DTO;

namespace Repository
{
    class BranchRepository
    {
        public List<DM.Branch> GetAllBranches()
        {
            return DataSource._branches
                .Where(m => m.IsActive = true)
                .Select((m) => new DM.Branch
                {
                    BranchID = m.tblBranchID,
                    AutoPull = m.AutoPull,
                    IsActive = m.IsActive,
                    Name = m.Name,
                    EnableDeskTopNotifications = m.EnableDeskTopNotifications,
                    RepoID = m.tblRepoID,
                    Repos = m.tblRepos.Select((n) => new DM.Repo
                    {
                        RepoID = n.tblRepoID,
                        Name = n.Name,
                        AutoTrack = n.AutoTrack,
                        CreatedAt = n.CreatedAt,
                        ModifiedAt = n.ModifiedAt,
                        RecentCheck = n.RecentCheck,
                        WorkingDirectory = n.WorkingDirectory,
                        IsActive = n.IsActive,
                        EnableDesktopNotification = n.EnableDesktopNotification,
                    }).Where(o => o.RepoID == m.tblRepoID).ToList()
                }
                )
                .ToList();
        }
    }
}