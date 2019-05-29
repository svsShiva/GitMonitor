using GitMonitor.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    public class RepoRepository
    {
        public List<DM.Repo> GetAllTrackedRepos()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblRepo>()
                        .Where(m => m.IsActive && !m.IsUntrackedRepo)
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
                            EnableEmailNotification = m.EnableEmailNotification,
                            EmailGroupIDS = m.EmailGroupIDS,
                            CurrentBranch = m.CurrentBranch,
                            IsUntrackedRepo = m.IsUntrackedRepo,
                            Branches = db.Table<tblBranch>().Where(n => n.tblRepoID == m.tblRepoID)
                            .Select((n) => new DM.Branch
                            {
                                BranchID = n.tblBranchID,
                                AutoPull = n.AutoPull,
                                IsActive = n.IsActive,
                                Name = n.Name,
                                EnableDesktopNotification = n.EnableDesktopNotification,
                                EnableEmailNotification = n.EnableEmailNotification,
                                RepoID = n.tblRepoID,
                                AheadBy = n.AheadBy,
                                BehindBy = n.BehindBy,
                                HasUpstream = n.HasUpstream,
                                Remote = n.Remote,
                                TrackingBranch = n.TrackingBranch,
                                SendDesktopNoti = n.SendDesktopNoti,
                                SendEmailNoti = n.SendEmailNoti
                            }).Where(o => o.RepoID == m.tblRepoID).ToList()
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public List<DM.Repo> GetAllUnTrackedRepos()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblRepo>()
                        .Where(m => m.IsActive && m.IsUntrackedRepo)
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
                            EnableEmailNotification = m.EnableEmailNotification,
                            EmailGroupIDS = m.EmailGroupIDS,
                            CurrentBranch = m.CurrentBranch,
                            IsUntrackedRepo = m.IsUntrackedRepo,
                            Branches = db.Table<tblBranch>().Where(n => n.tblRepoID == m.tblRepoID)
                            .Select((n) => new DM.Branch
                            {
                                BranchID = n.tblBranchID,
                                AutoPull = n.AutoPull,
                                IsActive = n.IsActive,
                                Name = n.Name,
                                EnableDesktopNotification = n.EnableDesktopNotification,
                                EnableEmailNotification = n.EnableEmailNotification,
                                RepoID = n.tblRepoID,
                                AheadBy = n.AheadBy,
                                BehindBy = n.BehindBy,
                                HasUpstream = n.HasUpstream,
                                Remote = n.Remote,
                                TrackingBranch = n.TrackingBranch
                            }).Where(o => o.RepoID == m.tblRepoID).ToList()
                        }
                        )
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void Delete(string path)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    //TODO Why condition is based on path
                    //TODO Write comments
                    var rec = db.Table<tblRepo>()
                                .Where(m => m.WorkingDirectory == path)
                                .FirstOrDefault();

                    // repo already exists and isActive=true
                    if (rec != null && rec.IsActive != false)
                    {
                        rec.IsActive = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void Add(DM.Repo repo)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    var rec = db.Table<tblRepo>()
                                .Where(m => m.WorkingDirectory == repo.WorkingDirectory)
                                .FirstOrDefault();

                    // repo already exists and isActive=false
                    if (rec != null && rec.IsActive == false)
                    {
                        rec.IsActive = true;
                        rec.IsUntrackedRepo = true;
                    }

                    if (rec == null)
                    {
                        db.Insert(new tblRepo
                        {
                            WorkingDirectory = repo.WorkingDirectory,
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            AutoTrack = false,
                            IsUntrackedRepo = true,
                            EnableDesktopNotification = false,
                            ModifiedAt = DateTime.Now,
                            RecentCheck = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void AddMultiple(List<string> repos)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    var DBRepos = db.Table<tblRepo>().ToList();

                    foreach (var repo in repos)
                    {
                        if (!DBRepos.Any(m => m.WorkingDirectory == repo))
                        {
                            db.Insert(new tblRepo
                            {
                                WorkingDirectory = repo,
                                IsActive = true,
                                CreatedAt = DateTime.Now,
                                AutoTrack = false,
                                IsUntrackedRepo = true,
                                EnableDesktopNotification = false,
                                ModifiedAt = DateTime.Now,
                                RecentCheck = DateTime.Now
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void Update(DM.Repo repo)
        {
            using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
            {
                //TODO check for working directory logic
                tblRepo tblRepoObj = db.Table<tblRepo>()
                                       .FirstOrDefault(m => m.WorkingDirectory == repo.WorkingDirectory);

                if (tblRepoObj == null)
                {
                    //TODO throw exception
                    return;
                }

                tblRepoObj.Name = repo.Name;
                tblRepoObj.EmailGroupIDS = repo.EmailGroupIDS;
                tblRepoObj.CurrentBranch = repo.CurrentBranch;
                tblRepoObj.EnableDesktopNotification = repo.EnableDesktopNotification;
                tblRepoObj.AutoTrack = repo.AutoTrack;
                tblRepoObj.ModifiedAt = DateTime.Now;
                tblRepoObj.RepoUrl = repo.RepoUrl;
                tblRepoObj.EnableEmailNotification = repo.EnableEmailNotification;
                tblRepoObj.IsUntrackedRepo = repo.IsUntrackedRepo;
                tblRepoObj.RecentCheck = DateTime.Now;

                repo.RecentCheck = tblRepoObj.RecentCheck;

                foreach (var branch in repo.Branches)
                {
                    tblBranch tblBranchObj = db.Table<tblBranch>()
                                       .FirstOrDefault(m => m.tblBranchID == branch.BranchID);

                    if (tblBranchObj == null)
                    {
                        tblBranchObj = new tblBranch();
                    }

                    tblBranchObj.tblRepoID = repo.RepoID;
                    tblBranchObj.AutoPull = branch.AutoPull;
                    tblBranchObj.EnableDesktopNotification = branch.EnableDesktopNotification;
                    tblBranchObj.EnableEmailNotification = branch.EnableEmailNotification;
                    tblBranchObj.IsActive = branch.IsActive;
                    tblBranchObj.Name = branch.Name;
                    tblBranchObj.HasUpstream = branch.HasUpstream;
                    tblBranchObj.Remote = branch.Remote;
                    tblBranchObj.TrackingBranch = branch.TrackingBranch;
                    tblBranchObj.AheadBy = branch.AheadBy;
                    tblBranchObj.BehindBy = branch.BehindBy;
                    tblBranchObj.SendEmailNoti = branch.SendEmailNoti;
                    tblBranchObj.SendDesktopNoti = branch.SendDesktopNoti;

                    if (tblBranchObj.tblBranchID == 0)
                    {
                        db.Insert(tblBranchObj);
                    }
                    else
                    {
                        db.Update(tblBranchObj);
                    }
                }

                db.Update(tblRepoObj);
            }
        }

        public DM.Repo GetRepoByID(long id)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    //TODO need to update branches logic, inner Query
                    return db.Table<tblRepo>()
                        .Where(m => m.tblRepoID == id)
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
                            EnableEmailNotification = m.EnableEmailNotification,
                            EmailGroupIDS = m.EmailGroupIDS,
                            CurrentBranch = m.CurrentBranch,
                            Branches = db.Table<tblBranch>().Where(n => n.tblRepoID == m.tblRepoID)
                            .Select((n) => new DM.Branch
                            {
                                BranchID = n.tblBranchID,
                                AutoPull = n.AutoPull,
                                IsActive = n.IsActive,
                                Name = n.Name,
                                EnableDesktopNotification = n.EnableDesktopNotification,
                                EnableEmailNotification = n.EnableEmailNotification,
                                RepoID = n.tblRepoID,
                                AheadBy = n.AheadBy,
                                BehindBy = n.BehindBy,
                                HasUpstream = n.HasUpstream,
                                Remote = n.Remote,
                                TrackingBranch = n.TrackingBranch

                            }).Where(o => o.RepoID == m.tblRepoID).ToList()
                        }
                        )
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public bool StopTrackingRepo(long id)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    var rec = db.Table<tblRepo>()
                                .Where(m => m.tblRepoID == id)
                                .FirstOrDefault();

                    // repo already exists and isActive=true
                    if (rec != null)
                    {
                        rec.IsUntrackedRepo = true;
                        db.Update(rec);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public List<DM.Repo> GetReposForNotifications()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblRepo>()
                        .Where(m => (m.IsActive && !m.IsUntrackedRepo) && (m.EnableDesktopNotification || m.EnableEmailNotification))
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
                            EnableEmailNotification = m.EnableEmailNotification,
                            EmailGroupIDS = m.EmailGroupIDS,
                            CurrentBranch = m.CurrentBranch,
                            IsUntrackedRepo = m.IsUntrackedRepo,
                            Branches = db.Table<tblBranch>().Where(o => o.tblRepoID == m.tblRepoID && (m.EnableDesktopNotification || m.EnableEmailNotification))
                            .Select((n) => new DM.Branch
                            {
                                BranchID = n.tblBranchID,
                                AutoPull = n.AutoPull,
                                IsActive = n.IsActive,
                                Name = n.Name,
                                EnableDesktopNotification = n.EnableDesktopNotification,
                                EnableEmailNotification = n.EnableEmailNotification,
                                RepoID = n.tblRepoID,
                                AheadBy = n.AheadBy,
                                BehindBy = n.BehindBy,
                                HasUpstream = n.HasUpstream,
                                Remote = n.Remote,
                                TrackingBranch = n.TrackingBranch,
                                SendEmailNoti = n.SendEmailNoti,
                                SendDesktopNoti = n.SendDesktopNoti
                            }).ToList()
                        })
                        .ToList().Where(m => m.Branches.Any(n => n.SendDesktopNoti || n.SendEmailNoti)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void UpdateNotiFlag(long repoID, bool isDesktopNoti)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    List<tblBranch> tblBranches = db.Table<tblBranch>()
                                                    .Where(m => m.tblRepoID == repoID)
                                                    .ToList();

                    foreach (var branch in tblBranches)
                    {
                        if (branch != null)
                        {
                            if (branch.SendDesktopNoti && isDesktopNoti)
                            {
                                branch.SendDesktopNoti = false;
                            }
                            else if (branch.SendEmailNoti)
                            {
                                branch.SendEmailNoti = false;
                            }
                            db.Update(branch);
                        }
                    }
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