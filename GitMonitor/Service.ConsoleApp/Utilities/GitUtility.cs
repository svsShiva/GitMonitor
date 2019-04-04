using GitMonitor.DomainModel.DTO;
using System;
using System.Linq;
using GitMonitor.Service.ConsoleApp.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    class GitUtility
    {
        static string Git { get { return "git {0}"; } }
        static string GitFetch { get { return string.Format(Git, "fetch"); } }
        static string GitBranchVV { get { return string.Format(Git, "branch -vv"); } }
        public static string GitStash { get { return string.Format(Git, "stash"); } }
        public static string GitPull { get { return string.Format(Git, "pull "); } }

        public static void RunTasks(Repo repo)
        {
            try
            {
                //Checking if any of the files in the repo got modified
                //If the modified time is less than "LastModifiedRunInterval" then we will skip the process assuming the repo is in use
                if (ChangesAreRecent(repo.WorkingDirectory))
                {
                    repo.RecentCheck = DateTime.Now;
                    return;
                }

                // Getting information about all the branches and their changes from upstream
                FetchCommand(repo);

                // It will fetch the status(ahead/behind, tracking braches) from upstream
                string[] branchVVOP = BranchVVCommand(repo);

                if (branchVVOP != null)
                {
                    // branchvvOP first element is the current branch
                    // Setting the current branch
                    string currBranchName = branchVVOP[0].Substring(2, branchVVOP[0].IndexOf(' ', 2));
                    if (currBranchName.Equals("(HEAD"))
                    {
                        string[] splits = branchVVOP[0].Split(' ');
                        if (splits[2].Equals("detached"))
                        {
                            currBranchName += $" {splits[1]} {splits[2]} {splits[3]}";
                            currBranchName = currBranchName.Substring(0, currBranchName.Length - 1);
                        }
                    }

                    repo.CurrentBranch = currBranchName;

                    SetNewBranches(repo, branchVVOP);

                    IEnumerable<string> localBranchesWithUpStreams = GetUpstreamsFromConfig(repo.WorkingDirectory);

                    //Retrieve upstreamed branches from config
                    foreach (var branch in repo.Branches)
                    {
                        // If branch doesnt have any upstream (implies that is an local branch)
                        // Updating branch information
                        if (branch.IsActive && !localBranchesWithUpStreams.Contains(branch.Name))
                        {
                            branch.HasUpstream = false;
                            branch.AheadBy = 0;
                            branch.BehindBy = 0;
                            branch.Remote = "";
                            branch.TrackingBranch = "";
                        }

                        // Check if the branch is present in branchVVOP
                        // If it is present getting the status
                        string vvBranch = branchVVOP.FirstOrDefault(x => x.StartsWith(branch.Name));

                        if (vvBranch == null)
                        {
                            branch.IsActive = false;
                        }
                        else if (branch.HasUpstream)
                        {
                            // get remote, remotebranch, ahead/behind
                            SetStatus(branch, vvBranch);

                            // 
                            if (branch.AutoPull)
                            {
                                //
                                if (branch.AheadBy == 0 && branch.BehindBy > 0)
                                {
                                    if (branch.Name == repo.CurrentBranch)
                                    {
                                        StashCommand(repo);
                                    }

                                    PullCommand(repo, branch);
                                }
                                else if (branch.AheadBy > 0 && branch.BehindBy == 0)
                                {
                                    //Notify that you are ahead. Please push your changes
                                }
                                else if (branch.AheadBy > 0 && branch.BehindBy > 0)
                                {
                                    //Notify that you have diverged. Please merge manually.
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }


        private static void FetchCommand(Repo repo)
        {
            try
            {
                ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitFetch);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private static string[] BranchVVCommand(Repo repo)
        {
            try
            {
                string[] output = ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitBranchVV).Split('\n');
                return FormatBrancVVOutPut(output);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }

            return null;
        }

        private static void StashCommand(Repo repo)
        {
            try
            {
                ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitStash);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private static void PullCommand(Repo repo, Branch branch)
        {
            try
            {
                ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitPull + $"{branch.Remote} {branch.TrackingBranch}:{branch.Name}");
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }



        private static void SetStatus(Branch branch, string branchvvline)
        {
            branch.AheadBy = 0;
            branch.BehindBy = 0;

            int brNameLimit = 0;

            while (branchvvline[brNameLimit] != ' ') brNameLimit++;
            while (branchvvline[brNameLimit] == ' ') brNameLimit++;
            int commitEnd = branchvvline.IndexOf(' ', brNameLimit);

            int upStreamEnd = branchvvline.IndexOf(' ', commitEnd + 1);

            if (branchvvline[upStreamEnd - 1] == ':')
            {
                int space1 = branchvvline.IndexOf(' ', upStreamEnd + 1);
                int space2 = branchvvline.IndexOf(' ', space1 + 1);
                string key = branchvvline.Substring(upStreamEnd + 1, space1 - upStreamEnd - 1);
                int value = Convert.ToInt32(branchvvline.Substring(space1 + 1, space2 - space1 - 2));
                if (key.Equals("ahead"))
                {
                    branch.AheadBy = value;
                }
                else if (key.Equals("behind"))
                {
                    branch.BehindBy = value;
                }
                if (branchvvline[space2 - 1] == ',')
                {
                    int space3 = branchvvline.IndexOf(' ', space2 + 1);
                    int space4 = branchvvline.IndexOf(' ', space3 + 1);
                    key = branchvvline.Substring(space2 + 1, space3 - space2 - 1);
                    value = Convert.ToInt32(branchvvline.Substring(space3 + 1, space4 - space3 - 2));
                    if (key.Equals("ahead"))
                    {
                        branch.AheadBy = value;
                    }
                    else if (key.Equals("behind"))
                    {
                        branch.BehindBy = value;
                    }
                }
            }

            string remotebranch = branchvvline.Substring(commitEnd + 2, upStreamEnd - commitEnd - 3);
            branch.Remote = remotebranch.Split('/')[0];
            branch.TrackingBranch = remotebranch.Split('/')[1];
        }

        private static IEnumerable<string> GetUpstreamsFromConfig(string workingDir)
        {
            string configop = File.ReadAllText(workingDir + ".git/config");
            RemoveSpaces(configop);

            List<string> result = new List<string>();
            foreach (var line in configop.Split(new[] { "[branch\"" }, StringSplitOptions.RemoveEmptyEntries))
            {
                int remoteIndex = line.IndexOf("remote=");
                result.Add(line.Substring(remoteIndex, line.LastIndexOf('\"', 0, remoteIndex)));
            }

            return result;
        }

        private static void RemoveSpaces(string catop)
        {
            int spaceIndex;
            while ((spaceIndex = catop.IndexOf(' ')) != -1)
            {
                catop.Remove(spaceIndex, 1);
            }
        }

        private static void SetNewBranches(Repo repo, string[] branchVVOP)
        {
            foreach (var opline in branchVVOP)
            {
                if (opline.StartsWith("(HEAD det"))
                {
                    continue;
                }

                string branchName = opline.Split(' ')[0];

                if (repo.Branches.Any(m => m.Name.Equals(branchName)))
                {
                    repo.Branches.Add(new Branch
                    {
                        Name = branchName,
                        EnableDeskTopNotifications = false,
                        AutoPull = false,
                        IsActive = true,
                        HasUpstream = false
                    });
                }
            }
        }

        private static string[] FormatBrancVVOutPut(string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                string currBranchName = string.Empty;
                if (list[i][0] == '*')
                {
                    currBranchName = list[i].Substring(2, list[i].IndexOf(' ', 2));
                    if (currBranchName.Equals("(HEAD"))
                    {
                        string[] splits = list[i].Split(' ');
                        if (splits[2].Equals("detached"))
                        {
                            currBranchName += $" {splits[1]} {splits[2]} {splits[3]}";
                            currBranchName = currBranchName.Substring(0, currBranchName.Length - 1);
                        }
                    }
                }
                list[i] = list[i].Substring(2);
            }

            return list;
        }

        private static bool ChangesAreRecent(string directory)
        {
            if (Directory.GetLastAccessTime(directory) > DateTime.Now - TimeSpan
                                 .FromMinutes(Convert.ToInt16(ConfigurationManager.AppSettings["LastModifiedRunInterval"].ToString())))
            {
                return true;
            }

            string[] innerDirectories = Directory.GetDirectories(directory);
            foreach (var innerDirectory in innerDirectories)
            {
                if (ChangesAreRecent(innerDirectory))
                {
                    return true;
                }
            }

            return false;
        }
    }
}