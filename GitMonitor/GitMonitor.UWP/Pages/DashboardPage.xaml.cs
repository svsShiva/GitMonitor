using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using GitMonitor.UWP.ViewModels;
using System;
using System.Collections.Generic;
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

        public List<RepoViewModel> RepoViewModels { get; set; }

        public List<Repo> Repos { get; set; }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                Repos = await APIUtility.Get<List<Repo>>(RouteUtility._getGetAllTrackedRepos);

                //RepoViewModels = ModelConversionUtility.RepoListToViewModelList(Repos);
                dgDashboard.ItemsSource = Repos;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void abbtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void abbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //TODO check for more optimised logic
            AppBarButton item = (AppBarButton)sender;

            Repo repo = (Repo)item.DataContext;

            AddEditRepoDialog addEditRepoDialog = new AddEditRepoDialog("Edit - " + repo.Name, repo);
            addEditRepoDialog.ShowAsync();
        }

        private void abbtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
