using GitMonitor.DataModel;
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
                        CurrentBranch = m.CurrentBranch,
                        Branches = m.tblBranches.Select((n) => new DM.Branch
                        {
                            BranchID = n.tblBranchID,
                            AutoPull = n.AutoPull,
                            IsActive = n.IsActive,
                            Name = n.Name,
                            EnableDeskTopNotifications = n.EnableDeskTopNotifications,
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
                        CurrentBranch = m.CurrentBranch,
                        Branches = m.tblBranches.Select((n) => new DM.Branch
                        {
                            BranchID = n.tblBranchID,
                            AutoPull = n.AutoPull,
                            IsActive = n.IsActive,
                            Name = n.Name,
                            EnableDeskTopNotifications = n.EnableDeskTopNotifications,
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

        //public List<DMVM.RepoViewModel> GetAllTrackedReposViewModel()
        //{
        //    List<DM.Repo> repos = GetAllTrackedRepos();
        //    List<DMVM.RepoViewModel> repoViewModelList = new List<DMVM.RepoViewModel>();

        //    foreach (var repo in repos)
        //    {
        //        string str = string.Empty;
        //        DMVM.RepoViewModel repoViewModel = new DMVM.RepoViewModel();
        //        repoViewModel.RepoID = repo.RepoID;
        //        repoViewModel.AutoTrack = repo.AutoTrack;
        //        repoViewModel.CreatedAt = repo.CreatedAt;
        //        repoViewModel.CurrentBranch = repo.CurrentBranch;
        //        repoViewModel.IsUntrackedRepo = repo.IsUntrackedRepo;
        //        repoViewModel.ModifiedAt = repo.ModifiedAt;
        //        repoViewModel.Name = repo.Name;
        //        repoViewModel.WorkingDirectory = repo.WorkingDirectory;
        //        repoViewModel.RecentCheck = repo.RecentCheck;

        //        if (repo.Branches.Count > 0)
        //        {
        //            foreach (var branch in repo.Branches)
        //            {
        //                str += "\n" + branch.Name;
        //                if (branch.HasUpstream)
        //                    str += $" (ahead {branch.AheadBy}, behind {branch.BehindBy})";
        //            }
        //        }
        //        else
        //        {
        //            str = "\nCould not detect any branches";
        //        }
        //        repoViewModel.BranchNames = str.Substring(1);

        //        repoViewModelList.Add(repoViewModel);
        //    }

        //    return repoViewModelList;
        //}

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
                        CurrentBranch = m.CurrentBranch,
                        Branches = m.tblBranches.Select((n) => new DM.Branch
                        {
                            BranchID = n.tblBranchID,
                            AutoPull = n.AutoPull,
                            IsActive = n.IsActive,
                            Name = n.Name,
                            EnableDeskTopNotifications = n.EnableDeskTopNotifications,
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
                        CurrentBranch = m.CurrentBranch,
                        Branches = m.tblBranches.Select((n) => new DM.Branch
                        {
                            BranchID = n.tblBranchID,
                            AutoPull = n.AutoPull,
                            IsActive = n.IsActive,
                            Name = n.Name,
                            EnableDeskTopNotifications = n.EnableDeskTopNotifications,
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
                        ModifiedAt = DateTime.Now,
                        RecentCheck = DateTime.Now
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
                            ModifiedAt = DateTime.Now,
                            RecentCheck = DateTime.Now
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

        public void Update(DM.Repo repo)
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                tblRepo tblRepoObj = db.tblRepoes.Include("tblBranches")
                                       .FirstOrDefault(m => m.WorkingDirectory == repo.WorkingDirectory);

                if (tblRepoObj == null)
                {
                    return;
                }

                tblRepoObj.RecentCheck = DateTime.Now;
                tblRepoObj.NotificationEmail = repo.NotificationEmail;
                tblRepoObj.RecentCheck = DateTime.Now;
                tblRepoObj.CurrentBranch = repo.CurrentBranch;
                tblRepoObj.EnableDesktopNotification = repo.EnableDesktopNotification;
                tblRepoObj.AutoTrack = repo.AutoTrack;
                tblRepoObj.ModifiedAt = repo.ModifiedAt;
                tblRepoObj.RepoUrl = repo.RepoUrl;

                foreach (var branch in repo.Branches)
                {
                    var tblBranchObj = tblRepoObj.tblBranches
                                       .FirstOrDefault(m => m.tblBranchID == branch.BranchID);

                    if (tblBranchObj == null)
                    {
                        tblBranchObj = new tblBranch();
                        //tblBranchObj.tblRepo = tblRepoObj; // Why is this line necessary?
                        tblRepoObj.tblBranches.Add(tblBranchObj);
                    }

                    tblBranchObj.AutoPull = branch.AutoPull;
                    tblBranchObj.EnableDeskTopNotifications = branch.EnableDeskTopNotifications;
                    tblBranchObj.IsActive = branch.IsActive;
                    tblBranchObj.Name = branch.Name;
                    tblBranchObj.HasUpstream = branch.HasUpstream;
                    tblBranchObj.Remote = branch.Remote;
                    tblBranchObj.TrackingBranch = branch.TrackingBranch;
                    tblBranchObj.AheadBy = branch.AheadBy;
                    tblBranchObj.BehindBy = branch.BehindBy;
                }

                db.SaveChanges();
            }
        }

        public DM.Repo GetRepoByID(long id)
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
                        CurrentBranch = m.CurrentBranch,
                        Branches = m.tblBranches.Select((n) => new DM.Branch
                        {
                            BranchID = n.tblBranchID,
                            AutoPull = n.AutoPull,
                            IsActive = n.IsActive,
                            Name = n.Name,
                            EnableDeskTopNotifications = n.EnableDeskTopNotifications,
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

        //public void UpdateFromUI(DMVM.EditFormViewModel repo)
        //{
        //    using (GitMonitorEntities db = new GitMonitorEntities())
        //    {
        //        tblRepo tblRepoObj = db.tblRepoes.Include("tblBranches")
        //                               .FirstOrDefault(m => m.WorkingDirectory == repo.WorkingDirectory);

        //        if (tblRepoObj == null)
        //        {
        //            throw new Exception("Record not found");
        //        }

        //        tblRepoObj.Name = repo.RepoName;
        //        tblRepoObj.EnableDesktopNotification = repo.EnableDesktopNotification;
        //        tblRepoObj.AutoTrack = repo.AutoTrack;
        //        tblRepoObj.RepoUrl = repo.RepoURL;
        //        tblRepoObj.IsUntrackedRepo = false;

        //        foreach (var branch in repo.Branches)
        //        {
        //            var tblBranchObj = tblRepoObj.tblBranches
        //                              .FirstOrDefault(m => m.tblBranchID == branch.BranchID);

        //            tblBranchObj.AutoPull = branch.AutoPull;
        //            tblBranchObj.EnableDeskTopNotifications = branch.EnableDeskTopNotifications;
        //        }

        //        db.SaveChanges();
        //    }
        //}

        //public List<DMVM.RepoViewModel> GetAllUnTrackedReposViewModel()
        //{
        //    List<DM.Repo> repos = GetAllUnTrackedRepos();
        //    List<DMVM.RepoViewModel> repoViewModelList = new List<DMVM.RepoViewModel>();

        //    foreach (var repo in repos)
        //    {
        //        string str = string.Empty;
        //        DMVM.RepoViewModel repoViewModel = new DMVM.RepoViewModel();
        //        repoViewModel.RepoID = repo.RepoID;
        //        repoViewModel.AutoTrack = repo.AutoTrack;
        //        repoViewModel.CreatedAt = repo.CreatedAt;
        //        repoViewModel.CurrentBranch = repo.CurrentBranch;
        //        repoViewModel.IsUntrackedRepo = repo.IsUntrackedRepo;
        //        repoViewModel.ModifiedAt = repo.ModifiedAt;
        //        repoViewModel.Name = repo.Name;
        //        repoViewModel.WorkingDirectory = repo.WorkingDirectory;
        //        repoViewModel.RecentCheck = repo.RecentCheck;

        //        if (repo.Branches.Count > 0)
        //        {
        //            foreach (var branch in repo.Branches)
        //            {
        //                str += "\n" + branch.Name;
        //                if (branch.HasUpstream)
        //                    str += $" (ahead {branch.AheadBy}, behind {branch.BehindBy})";
        //            }
        //        }
        //        else
        //        {
        //            str = "\nCould not detect any branches";
        //        }

        //        repoViewModel.BranchNames = str.Substring(1);
        //        repoViewModelList.Add(repoViewModel);
        //    }
        //    return repoViewModelList;
        //}

        public bool StopTrackingRepo(long id)
        {
            using (GitMonitorEntities db = new GitMonitorEntities())
            {
                var rec = db.tblRepoes.Where(m => m.tblRepoID == id).FirstOrDefault();

                // repo already exists and isActive=true
                if (rec != null)
                {
                    rec.IsUntrackedRepo = true;
                    db.SaveChanges();
                }
            }

            return true;
        }

        //public DMVM.EditFormViewModel GetUIRepoByID(long id)
        //{
        //    using (GitMonitorEntities db = new GitMonitorEntities())
        //    {
        //        return db.tblRepoes
        //            .Where(m => m.IsActive && m.tblRepoID == id)
        //            .Select((m) => new DMVM.EditFormViewModel
        //            {
        //                RepoID = m.tblRepoID,
        //                RepoName = m.Name,
        //                AutoTrack = m.AutoTrack,
        //                WorkingDirectory = m.WorkingDirectory,
        //                EnableDesktopNotification = m.EnableDesktopNotification,
        //                Branches = m.tblBranches.Select((n) => new DM.Branch
        //                {
        //                    BranchID = n.tblBranchID,
        //                    AutoPull = n.AutoPull,
        //                    IsActive = n.IsActive,
        //                    Name = n.Name,
        //                    EnableDeskTopNotifications = n.EnableDeskTopNotifications,
        //                    RepoID = n.tblRepoID,
        //                    AheadBy = n.AheadBy,
        //                    BehindBy = n.BehindBy,
        //                    HasUpstream = n.HasUpstream,
        //                    Remote = n.Remote,
        //                    TrackingBranch = n.TrackingBranch

        //                }).Where(o => o.RepoID == m.tblRepoID).ToList()
        //            }
        //            )
        //            .FirstOrDefault();
        //    }
        //}
    }
}