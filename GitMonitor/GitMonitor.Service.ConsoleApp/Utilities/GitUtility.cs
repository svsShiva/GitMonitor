using GitMonitor.DomainModel.DTO;
using System;
using System.Linq;
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

        public static void RunTasks(Repo repo, bool IgnoreRecentChanges)
        {
            try
            {
                //Checking if any of the files in the repo got modified
                //If the modified time is less than "LastModifiedRunInterval" then we will skip the autopull process assuming the repo is in use
                bool autoPullInCycle = IgnoreRecentChanges || ChangesAreRecent(repo.WorkingDirectory, TimeSpan
                                 .FromMinutes(Convert.ToInt16(ConfigurationManager.AppSettings["LastModifiedRunInterval"].ToString())));

                // Getting information about all the branches and their changes from upstream
                FetchCommand(repo);

                // It will fetch the status(ahead/behind, tracking braches) from upstream
                string[] branchVVOP = BranchVVCommand(repo);

                if (branchVVOP != null)
                {
                    // branchvvOP first element is the current branch
                    // Setting the current branch
                    string currBranchName = "";
                    if (branchVVOP.Length != 0)
                    {
                        currBranchName = branchVVOP[0].Substring(0, branchVVOP[0].IndexOf(' '));
                    }
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

                            // If the branch is set to be autopulled, autopull it.
                            if (autoPullInCycle && !repo.IsUntrackedRepo && branch.AutoPull)
                            {
                                // fast-forward only
                                if (branch.AheadBy == 0 && branch.BehindBy > 0)
                                {
                                    string op = null;
                                    if (branch.Name == repo.CurrentBranch)
                                    {
                                        StashCommand(repo);
                                        // git pull remoteName trackingBranchName:localBranchName
                                        op = PullCommand(repo, branch, GitPull);
                                    }
                                    else
                                    {
                                        // git fetch remoteName trackingBranchName:localBranchName
                                        op = PullCommand(repo, branch, GitFetch);
                                    }
                                    string newStatus = BranchVVCommand(repo).FirstOrDefault(x => x.StartsWith(branch.Name));
                                    if (newStatus == null)
                                    {
                                        branch.IsActive = false;
                                    }
                                    else
                                    {
                                        SetStatus(branch, newStatus);
                                    }
                                }
                            }
                        }
                    }
                }
                Repository.RepoRepository repository = new Repository.RepoRepository();
                repository.Update(repo);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private static void FetchCommand(Repo repo)
        {
            ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitFetch);

        }

        private static string[] BranchVVCommand(Repo repo)
        {
            string[] output = ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitBranchVV).Split('\n');
            return FormatBrancVVOutPut(output);
        }

        private static void StashCommand(Repo repo)
        {
            ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitStash);
        }

        private static string PullCommand(Repo repo, Branch branch, string command)
        {
            return ProcessUtility.ExecuteCommand(repo.WorkingDirectory, command + $" {branch.Remote} {branch.TrackingBranch}:{branch.Name}");
        }

        private static void SetStatus(Branch branch, string branchvvline)
        {
            int brNameLimit = 0;
            // branchvvline will be of the following format:
            // features         7eca8f7 [origin/features] New File Added from another dir
            // {brName}        {commit#}  {tracking info}     {commit msg}
            while (branchvvline[brNameLimit] != ' ') brNameLimit++;
            while (branchvvline[brNameLimit] == ' ') brNameLimit++;

            // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
            //                 ^brNameLimit
            int commitEnd = branchvvline.IndexOf(' ', brNameLimit);

            // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
            //                         ^commitEnd 

            int upStreamEnd = branchvvline.IndexOf(' ', commitEnd + 1);

            // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
            //                         ^commitEnd        ^upStreamEnd     

            //branch.AheadBy = 0;
            //branch.BehindBy = 0;

            if (branchvvline[upStreamEnd - 1] == ':')
            {
                //                               upStreamEnd v        v space2
                // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
                //                                                 ^space1
                int space1 = branchvvline.IndexOf(' ', upStreamEnd + 1);
                int space2 = branchvvline.IndexOf(' ', space1 + 1);
                string key = branchvvline.Substring(upStreamEnd + 1, space1 - upStreamEnd - 1);
                int value = Convert.ToInt32(branchvvline.Substring(space1 + 1, space2 - space1 - 2));
                //                                            (key)
                // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
                //                                                  ^(value = 1)
                if (key.Equals("ahead"))
                {
                    if (branch.AheadBy != value)
                    {
                        branch.AheadBy = value;
                        branch.SendEmailNoti = true;
                        branch.SendDesktopNoti = true;
                    }
                }
                else if (key.Equals("behind"))
                {
                    if (branch.BehindBy != value)
                    {
                        branch.BehindBy = value;
                        branch.SendEmailNoti = true;
                        branch.SendDesktopNoti = true;
                    }
                }
                // Possibility1:                                      v space2
                // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir

                // Possibility2:                                      v space2
                // features         7eca8f7 [origin/features: ahead 1] New File Added from another dir
                if (branchvvline[space2 - 1] == ',')
                {
                    //                                             space2 v         v space4
                    // features         7eca8f7 [origin/features: ahead 1, behind 1] New File Added from another dir
                    //                                                     space3^
                    int space3 = branchvvline.IndexOf(' ', space2 + 1);
                    int space4 = branchvvline.IndexOf(' ', space3 + 1);
                    key = branchvvline.Substring(space2 + 1, space3 - space2 - 1);
                    value = Convert.ToInt32(branchvvline.Substring(space3 + 1, space4 - space3 - 2));
                    if (key.Equals("ahead"))
                    {
                        if (branch.AheadBy != value)
                        {
                            branch.AheadBy = value;
                            branch.SendEmailNoti = true;
                            branch.SendDesktopNoti = true;
                        }
                    }
                    else if (key.Equals("behind"))
                    {
                        if (branch.BehindBy != value)
                        {
                            branch.BehindBy = value;
                            branch.SendEmailNoti = true;
                            branch.SendDesktopNoti = true;
                        }
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
            configop = RemoveSpaces(configop);

            List<string> result = new List<string>();
            foreach (var line in configop.Split(new[] { "[branch\"" }, StringSplitOptions.RemoveEmptyEntries))
            {
                int branchNameLimiter = line.IndexOf('\"', 0);
                // Case occurs when there are no branches in the config file
                if (branchNameLimiter == -1)
                {
                    branchNameLimiter = 0;
                }
                // checking if branch has a remote
                int remoteIndex = line.IndexOf("remote=", branchNameLimiter);
                if (remoteIndex != -1)
                {
                    result.Add(line.Substring(0, branchNameLimiter));
                }
            }

            return result;
        }

        private static string RemoveSpaces(string catop)
        {
            int spaceIndex;
            while ((spaceIndex = catop.IndexOf(' ')) != -1)
            {
                catop = catop.Remove(spaceIndex, 1);
            }
            return catop;
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

                if (!repo.Branches.Any(m => m.Name.Equals(branchName)))
                {
                    repo.Branches.Add(new Branch
                    {
                        Name = branchName,
                        EnableDesktopNotification = false,
                        EnableEmailNotification = false,
                        AutoPull = false,
                        IsActive = true,
                        HasUpstream = true
                    });
                }
            }
        }

        private static string[] FormatBrancVVOutPut(string[] list)
        {
            list = list.TakeWhile(x => x != string.Empty).OrderByDescending(x => x[0]).Select(x => x.Substring(2)).ToArray();
            return list;
        }

        private static bool ChangesAreRecent(string directory, TimeSpan ignoreTime)
        {
            if (Directory.GetLastWriteTimeUtc(directory) > DateTime.UtcNow - ignoreTime)
            {
                return true;
            }

            IEnumerable<string> nonGitFiles = Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
                .Where(x => !x.StartsWith(directory + ".git\\")).ToList();
            foreach (var file in nonGitFiles)
            {
                if (File.GetLastWriteTimeUtc(file) > DateTime.UtcNow - ignoreTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}