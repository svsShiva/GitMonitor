using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using GitMonitor.UWP.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UntrackedReposPage : Page
    {
        public UntrackedReposPage()
        {
            this.InitializeComponent();
        }

        public List<Repo> Repos { get; set; }


        private async void Page_Loading(FrameworkElement sender, object args)
        {
            APIUtility APIUtility = new APIUtility();

            Repos = await APIUtility.Get<List<Repo>>(RouteUtility._getGetAllUnTrackedRepos);

            //RepoViewModels = ModelConversionUtility.RepoListToViewModelList(Repos);
            dgUnTrackedRepos.ItemsSource = Repos;
        }

        private void abbtnTrackRepo_Click(object sender, RoutedEventArgs e)
        {
            //TODO check for more optimised logic
            AppBarButton item = (AppBarButton)sender;

            Repo repo = (Repo)item.DataContext;

            AddEditRepoDialog addEditRepoDialog = new AddEditRepoDialog("Track Repo", repo);
            addEditRepoDialog.ShowAsync();
        }
    }
}
