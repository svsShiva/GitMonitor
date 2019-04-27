using GitMonitor.UWP.DTO;
using GitMonitor.UWP.ViewModels;
using System.Collections.Generic;

namespace GitMonitor.UWP.Utilities
{
    internal static class ModelConversionUtility
    {
        internal static RepoViewModel RepoToViewModel(Repo repo)
        {
            string str = string.Empty;
            RepoViewModel repoViewModel = new RepoViewModel();
            repoViewModel.RepoID = repo.RepoID;
            repoViewModel.AutoTrack = repo.AutoTrack;
            repoViewModel.CreatedAt = repo.CreatedAt;
            repoViewModel.CurrentBranch = repo.CurrentBranch;
            repoViewModel.IsUntrackedRepo = repo.IsUntrackedRepo;
            repoViewModel.ModifiedAt = repo.ModifiedAt;
            repoViewModel.Name = repo.Name;
            repoViewModel.WorkingDirectory = repo.WorkingDirectory;
            repoViewModel.RecentCheck = repo.RecentCheck;

            if (repo.Branches.Count > 0)
            {
                foreach (var branch in repo.Branches)
                {
                    str += "\n" + branch.Name;
                    if (branch.HasUpstream)
                        str += $" (ahead {branch.AheadBy}, behind {branch.BehindBy})";
                }
            }
            else
            {
                str = "\nCould not detect any branches";
            }
            repoViewModel.BranchNames = str.Substring(1);

            return repoViewModel;
        }

        internal static List<RepoViewModel> RepoListToViewModelList(List<Repo> repos)
        {
            List<RepoViewModel> list = new List<RepoViewModel>();

            foreach (var repo in repos)
            {
                list.Add(RepoToViewModel(repo));
            }

            return list;
        }

    }
}
