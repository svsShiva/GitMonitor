using DataModel;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel.DTO;

namespace Repository
{
    public class RepoRepository
    {
        public List<DM.Repo> GetAllRepos()
        {
            return DataSource._repos
                .Where(m => m.IsActive = true)
                .Select((m) => new DM.Repo
                {
                    RepoID = m.tblRepoID,
                    Name = m.Name,
                    AutoTrack = m.AutoTrack,
                    CreatedAt = m.CreatedAt,
                    ModifiedAt = m.ModifiedAt,
                    RecentCheck = m.RecentCheck,
                    WorkingDirectory = m.WorkingDirectory,
                    IsActive = m.IsActive,
                    EnableDesktopNotification = m.EnableDesktopNotification,
                    Branches = m.tblBranches.Select((n) => new DM.Branch
                    {
                        BranchID = n.tblBranchID,
                        AutoPull = n.AutoPull,
                        IsActive = n.IsActive,
                        Name = n.Name,
                        EnableDeskTopNotifications = n.EnableDeskTopNotifications,
                        RepoID = n.tblRepoID
                    }).Where(o => o.RepoID == m.tblRepoID).ToList()
                }
                )
                .ToList();
        }
    }
}