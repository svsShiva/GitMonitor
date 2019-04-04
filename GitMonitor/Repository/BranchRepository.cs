using GitMonitor.DataModel;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    class BranchRepository
    {
        public List<DM.Branch> GetAllBranches()
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                return db.tblBranches
                    .Where(m => m.IsActive == true)
                    .Select((m) => new DM.Branch
                    {
                        BranchID = m.tblBranchID,
                        AutoPull = m.AutoPull,
                        IsActive = m.IsActive,
                        Name = m.Name,
                        EnableDeskTopNotifications = m.EnableDeskTopNotifications,
                        RepoID = m.tblRepoID,
                    }).ToList();
            }
        }
    }
}