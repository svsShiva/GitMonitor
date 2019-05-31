using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GitMonitor.UWP.Pages
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
        }

        public List<Repo> Repos { get; set; }
        
        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                Repos = await APIUtility.Get<List<Repo>>(RouteUtility._getAllTrackedRepos);

                foreach (Repo repo in Repos)
                {
                    repo.IsAhead = repo.Branches.Any(m => m.AheadBy > 0);
                    repo.IsBehind = repo.Branches.Any(m => m.BehindBy > 0);
                    repo.IsUptoDate = repo.Branches.Any(m => m.AheadBy == 0 && m.BehindBy == 0);
                }

                dgDashboard.ItemsSource = null;
                dgDashboard.ItemsSource = Repos;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void RefreshSyncRepo(long id, string route)
        {
            APIUtility APIUtility = new APIUtility();

            Repo oldRepoObj = Repos.First(m => m.RepoID == id);
            Repo newRepoObj = await APIUtility.Get<Repo>(string.Format(route, id));

            Repos.Remove(oldRepoObj);
            Repos.Add(newRepoObj);

            //Updating branch status
            newRepoObj.IsAhead = newRepoObj.Branches.Any(m => m.AheadBy > 0);
            newRepoObj.IsBehind = newRepoObj.Branches.Any(m => m.BehindBy > 0);
            newRepoObj.IsUptoDate = newRepoObj.Branches.Any(m => m.AheadBy == 0 && m.BehindBy == 0);

            dgDashboard.ItemsSource = null;
            dgDashboard.ItemsSource = Repos;
        }

        private async void abbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo obj = ((FrameworkElement)sender).DataContext as Repo;

                RefreshSyncRepo(obj.RepoID, RouteUtility._refreshRepo);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {//TODO check for more optimised logic
                AppBarButton item = (AppBarButton)sender;

                Repo repo = (Repo)item.DataContext;

                AddEditRepoDialog addEditRepoDialog = new AddEditRepoDialog("Edit - " + repo.Name, repo);
                addEditRepoDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo obj = ((FrameworkElement)sender).DataContext as Repo;

                ConfirmationDialog confirmationDialog = new ConfirmationDialog(string.Format(StringUtility._repoUnTrackedConfirmation, obj.Name));

                ContentDialogResult contentDialogResult = await confirmationDialog.ShowAsync();

                if (contentDialogResult == ContentDialogResult.Primary)
                {
                    APIUtility APIUtility = new APIUtility();
                    APIUtility.Delete(string.Format(RouteUtility._stopTrackingRepo, obj.RepoID));

                    Repos.Remove(obj);
                    dgDashboard.ItemsSource = null;
                    dgDashboard.ItemsSource = Repos;

                    string message = string.Format(StringUtility._repoUntrackedSucessfully, obj.Name);
                    await new MessageDialog(message).ShowAsync();
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void hlBranches_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo repo = ((FrameworkElement)sender).DataContext as Repo;

                BranchesDialog branchesDialog = new BranchesDialog(repo.Branches);

                await branchesDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbSearch.Text.Length != 0)
                {
                    dgDashboard.ItemsSource = Repos.Where(m => m.Name.ToLower().Contains(tbSearch.Text)).ToList();
                }
                else
                {
                    dgDashboard.ItemsSource = Repos;
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo obj = ((FrameworkElement)sender).DataContext as Repo;

                RefreshSyncRepo(obj.RepoID, RouteUtility._syncRepo);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}
