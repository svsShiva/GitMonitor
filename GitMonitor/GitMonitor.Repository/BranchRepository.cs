using GitMonitor.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    class BranchRepository
    {
        public List<DM.Branch> GetAllBranches()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblBranch>()
                        .Where(m => m.IsActive == true)
                        .Select((m) => new DM.Branch
                        {
                            BranchID = m.tblBranchID,
                            AutoPull = m.AutoPull,
                            IsActive = m.IsActive,
                            Name = m.Name,
                            EnableDesktopNotification = m.EnableDesktopNotification,
                            EnableEmailNotification = m.EnableEmailNotification,
                            RepoID = m.tblRepoID,
                            AheadBy = m.AheadBy,
                            BehindBy = m.BehindBy,
                            HasUpstream = m.HasUpstream,
                            Remote = m.Remote,
                            TrackingBranch = m.TrackingBranch
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }
    }
}