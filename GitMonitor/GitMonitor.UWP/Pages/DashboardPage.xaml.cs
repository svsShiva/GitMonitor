﻿using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
        }

        public List<Repo> Repos { get; set; }

        public async void LoadData()
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

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                Repo obj = ((FrameworkElement)sender).DataContext as Repo;

                Repo oldRepoObj = Repos.First(m => m.RepoID == obj.RepoID);
                Repo newRepoObj = await APIUtility.Get<Repo>(string.Format(RouteUtility._refreshRepo, obj.RepoID));

                Repos.Remove(oldRepoObj);
                Repos.Add(newRepoObj);

                //Updating branch status
                newRepoObj.IsAhead = newRepoObj.Branches.Any(m => m.AheadBy > 0);
                newRepoObj.IsBehind = newRepoObj.Branches.Any(m => m.BehindBy > 0);
                newRepoObj.IsUptoDate = newRepoObj.Branches.Any(m => m.AheadBy == 0 && m.BehindBy == 0);

                dgDashboard.ItemsSource = null;
                dgDashboard.ItemsSource = Repos;
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

        private async void tbSearchRepoByName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbSearchRepoByName.Text.Length != 0)
                {
                    dgDashboard.ItemsSource = Repos.Where(m => m.Name.ToLower().Contains(tbSearchRepoByName.Text)).ToList();
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
    }
}
