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
                                       .FirstOrDefault(m => m.Name == branch.Name);

                    if (tblBranchObj == null)
                    {
                        tblBranchObj = new tblBranch();
                    }

                    tblBranchObj.tblRepoID = repo.RepoID;
                    tblBranchObj.AutoPull = branch.AutoPull;
                    tblBranchObj.EnableDesktopNotification = branch.EnableDesktopNotification;
                    tblBranchObj.EnableEmailNotification = repo.EnableEmailNotification;
                    tblBranchObj.IsActive = branch.IsActive;
                    tblBranchObj.Name = branch.Name;
                    tblBranchObj.HasUpstream = branch.HasUpstream;
                    tblBranchObj.Remote = branch.Remote;
                    tblBranchObj.TrackingBranch = branch.TrackingBranch;
                    tblBranchObj.AheadBy = branch.AheadBy;
                    tblBranchObj.BehindBy = branch.BehindBy;

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

        //public List<DM.Repo> GetAllDeletedRepos()
        //{
        //    //TODO need to check for usage, 0 references
        //    //TODO need to update branches logic, inner Query
        //    using (SQLiteConnection db = InitializeDB.Initialize())
        //    {
        //        return db.Table<tblRepo>()
        //            .Where(m => !m.IsActive)
        //            .Select((m) => new DM.Repo
        //            {
        //                RepoID = m.tblRepoID,
        //                Name = m.Name,
        //                AutoTrack = m.AutoTrack,
        //                CreatedAt = m.CreatedAt,
        //                ModifiedAt = m.ModifiedAt,
        //                RecentCheck = m.RecentCheck,
        //                WorkingDirectory = m.WorkingDirectory,
        //                IsActive = m.IsActive,
        //                EnableDesktopNotification = m.EnableDesktopNotification,
        //                CurrentBranch = m.CurrentBranch,
        //                Branches = db.Table<tblBranch>().Where(n => n.tblRepoID == m.tblRepoID)
        //                .Select((n) => new DM.Branch
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
        //            .ToList();
        //    }
        //}

        //public void UpdateFromUI(DMVM.EditFormViewModel repo)
        //{
        //    using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
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


        //public DMVM.EditFormViewModel GetUIRepoByID(long id)
        //{
        //    using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
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

        //public List<DM.Repo> GetAllActiveRepos()
        //{
        //    using (SQLiteConnection db = InitializeDB.Initialize())
        //    {
        //        //TODO need to update branches logic, inner Query
        //        return db.Table<tblRepo>()
        //            .Where(m => m.IsActive)
        //            .Select((m) => new DM.Repo
        //            {
        //                RepoID = m.tblRepoID,
        //                Name = m.Name,
        //                AutoTrack = m.AutoTrack,
        //                CreatedAt = m.CreatedAt,
        //                ModifiedAt = m.ModifiedAt,
        //                RecentCheck = m.RecentCheck,
        //                WorkingDirectory = m.WorkingDirectory,
        //                IsActive = m.IsActive,
        //                EnableDesktopNotification = m.EnableDesktopNotification,
        //                CurrentBranch = m.CurrentBranch,
        //                Branches = db.Table<tblBranch>().Where(n => n.tblRepoID == m.tblRepoID)
        //                .Select((n) => new DM.Branch
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
        //            .ToList();
        //    }
        //}
    }
}