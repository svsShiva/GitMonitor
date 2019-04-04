using DM = GitMonitor.DomainModel.DTO;
using GitMonitor.Repository;
using System.IO;
using System.Collections.Generic;
using System;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    class FileMonitorUtility
    {
        public FileMonitorUtility()
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

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created && !e.FullPath.Contains("$Recycle.Bin"))
            {
                DM.Repo repo = new DM.Repo { WorkingDirectory = e.FullPath.Replace(".git", "") };

                RepoRepository repoRepository = new RepoRepository();
                repoRepository.Add(repo);

                GitUtility.RunTasks(repo);
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted && !e.FullPath.Contains("$Recycle.Bin"))
            {
                RepoRepository repo = new RepoRepository();
                repo.Delete(e.FullPath.Replace(".git", ""));
            }
        }

        public void GetAllRepos()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<string> repos = new List<string>();

            foreach (var drive in allDrives)
            {
                // root level directories
                string[] allDirectories = Directory.GetDirectories(drive.ToString());

                foreach (var dir in allDirectories)
                {
                    try
                    {
                        var gitDirectories = Directory.GetDirectories(dir, "*/.git", SearchOption.AllDirectories);

                        foreach (var item in gitDirectories)
                        {
                            if (!item.Contains("$Recycle.Bin"))
                            {
                                repos.Add(item.Replace(".git", ""));
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }

            RepoRepository repoRepository = new RepoRepository();
            repoRepository.AddMultiple(repos);
        }
    }
}