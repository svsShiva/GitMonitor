using System;
using System.Collections.Generic;

namespace DataModel
{
    /// <summary>
    /// Later this will be replaced by SQLite DB
    /// </summary>
    class DataSource
    {
        public static List<tblRepo> _repos;
        public static List<tblBranch> _branches;
        public static List<tblSettings> _settings;

        static DataSource()
        {
            _repos = new List<tblRepo>();
            _branches = new List<tblBranch>();
            _settings = new List<tblSettings>();

            _repos.Add(new tblRepo
            {
                tblRepoID = 1,
                Name = "Repo1",
                AutoTrack = true,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                RecentCheck = DateTime.Now,
                WorkingDirectory = "C:/Repo1",
                IsActive = true,
                EnableDesktopNotification = false,
            });

            _repos.Add(new tblRepo
            {
                tblRepoID = 2,
                Name = "Repo2",
                AutoTrack = true,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                RecentCheck = DateTime.Now,
                WorkingDirectory = "C:/Repo2",
                IsActive = true,
                EnableDesktopNotification = false,
            });

            _branches.Add(new tblBranch
            {
                tblBranchID = 1,
                AutoPull = false,
                IsActive = true,
                Name = "Dev",
                EnableDeskTopNotifications = false,
                tblRepoID = 1
            });

            _branches.Add(new tblBranch
            {
                tblBranchID = 2,
                AutoPull = false,
                IsActive = true,
                Name = "Prod",
                EnableDeskTopNotifications = false,
                tblRepoID = 1
            });

            _branches.Add(new tblBranch
            {
                tblBranchID = 3,
                AutoPull = false,
                IsActive = true,
                Name = "UAT",
                EnableDeskTopNotifications = false,
                tblRepoID = 2
            });

            _branches.Add(new tblBranch
            {
                tblBranchID = 4,
                AutoPull = false,
                IsActive = true,
                Name = "Master",
                EnableDeskTopNotifications = false,
                tblRepoID = 2
            });

            _settings.Add(new tblSettings
            {
                tblSettingsID = 1,
                Key = "Interval",
                Value = "10",
                IsActive = true
            });

            _settings.Add(new tblSettings
            {
                tblSettingsID = 1,
                Key = "EnableDesktopNot",
                Value = "true",
                IsActive = true
            });

            _settings.Add(new tblSettings
            {
                tblSettingsID = 1,
                Key = "GlobalEmail",
                Value = "test@gmail.com",
                IsActive = true
            });
        }
    }
}