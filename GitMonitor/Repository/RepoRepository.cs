using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel.DTO;

namespace Repository
{
    public class RepoRepository
    {
        public List<DM.Repo> GetAllTrackedRepos()
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                return db.tblRepoes
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

        public List<DM.Repo> GetAllUnTrackedRepos()
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                return db.tblRepoes
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

        public List<DM.Repo> GetAllDeletedRepos()
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                return db.tblRepoes
                    .Where(m => !m.IsActive)
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

        public List<DM.Repo> GetAllActiveRepos()
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                return db.tblRepoes
                    .Where(m => m.IsActive)
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

        public void Delete(string path)
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                var rec = db.tblRepoes.Where(m => m.WorkingDirectory == path).FirstOrDefault();

                // repo already exists and isActive=true
                if (rec != null && rec.IsActive != false)
                {
                    rec.IsActive = false;
                    db.SaveChanges();
                }
            }
        }

        public void Add(DM.Repo repo)
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                var rec = db.tblRepoes.Where(m => m.WorkingDirectory == repo.WorkingDirectory).FirstOrDefault();

                // repo already exists and isActive=false
                if (rec != null && rec.IsActive == false)
                {
                    rec.IsActive = true;
                    rec.IsUntrackedRepo = true;

                    db.SaveChanges();
                }

                if (rec == null)
                {
                    db.tblRepoes.Add(new tblRepo
                    {
                        WorkingDirectory = repo.WorkingDirectory,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        AutoTrack = false,
                        IsUntrackedRepo = true,
                        EnableDesktopNotification = false,
                        ModifiedAt = DateTime.Now
                    });

                    db.SaveChanges();
                }
            }
        }

        public void AddMultiple(List<string> repos)
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                var DBRepos = db.tblRepoes.ToList();

                bool recAdded = false;

                foreach (var repo in repos)
                {
                    if (!DBRepos.Any(m => m.WorkingDirectory == repo))
                    {
                        db.tblRepoes.Add(new tblRepo
                        {
                            WorkingDirectory = repo,
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            AutoTrack = false,
                            IsUntrackedRepo = true,
                            EnableDesktopNotification = false,
                            ModifiedAt = DateTime.Now
                        });

                        recAdded = true;
                    }
                }

                if (recAdded)
                {
                    db.SaveChanges();
                }
            }
        }
    }
}