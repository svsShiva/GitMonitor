using DM = GitMonitor.DomainModel.DTO;
using GitMonitor.Repository;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    class FileMonitorUtility
    {
        public FileMonitorUtility()
        {
            try
            {
                GetAllRepos();

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (var item in allDrives)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(item.ToString());
                    watcher.Filter = "*/.git";
                    watcher.Created += Watcher_Created;
                    watcher.Deleted += Watcher_Deleted;
                    watcher.EnableRaisingEvents = true;
                    watcher.IncludeSubdirectories = true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType == WatcherChangeTypes.Created && !e.FullPath.Contains("$Recycle.Bin"))
                {
                    DM.Repo repo = new DM.Repo { WorkingDirectory = e.FullPath.Replace(".git", "") };

                    RepoRepository repoRepository = new RepoRepository();
                    repoRepository.Add(repo);

                    GitUtility.RunTasks(repo, true);
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType == WatcherChangeTypes.Deleted && !e.FullPath.Contains("$Recycle.Bin"))
                {
                    RepoRepository repo = new RepoRepository();
                    repo.Delete(e.FullPath.Replace(".git", ""));
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        public void GetAllRepos()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (var drive in allDrives)
            {
                AddAllReposInDirectory(drive.ToString());
            }
        }

        private void GetCurrentUserRepos()
        {
            string userDirPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                userDirPath = Directory.GetParent(userDirPath).ToString();
            }
            AddAllReposInDirectory(userDirPath);
        }

        private void AddAllReposInDirectory(string directory)
        {
            Dictionary<string, string> repos = new Dictionary<string, string>();

            string[] allDirectories = Directory.GetDirectories(directory);

            foreach (var dir in allDirectories)
            {
                try
                {
                    var gitDirectories = Directory.GetDirectories(dir, "*.git", SearchOption.AllDirectories).TakeWhile(x => x.EndsWith("\\.git"));

                    foreach (var item in gitDirectories)
                    {
                        if (!item.Contains("$Recycle.Bin"))
                        {
                            repos.Add(item.Replace(".git", ""), new DirectoryInfo(item).Parent.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            RepoRepository repoRepository = new RepoRepository();
            repoRepository.AddMultiple(repos);
        }
    }
}