using GitMonitor.UWP.DTO;
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

                Repos = await APIUtility.Get<List<Repo>>(RouteUtility._getGetAllTrackedRepos);

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

                Repo repo = Repos.First(m => m.RepoID == obj.RepoID);
                repo = await APIUtility.Get<Repo>(string.Format(RouteUtility._getRefreshRepo, obj.RepoID));

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
                APIUtility APIUtility = new APIUtility();

                Repo obj = ((FrameworkElement)sender).DataContext as Repo;

                APIUtility.Delete(string.Format(RouteUtility._getStopTrackingRepo, obj.RepoID));

                Repos.Remove(obj);

                dgDashboard.ItemsSource = null;
                dgDashboard.ItemsSource = Repos;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}
