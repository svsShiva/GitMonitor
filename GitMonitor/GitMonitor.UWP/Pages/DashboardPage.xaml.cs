using GitMonitor.UWP.Pages.Dialogs;
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

            RepoViewModels = new List<RepoViewModel> {
                new RepoViewModel
                {
                    Name = "repo1",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 0, behind - 1)"
                },
                new RepoViewModel
                {
                    Name = "repo2",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 2, behind - 1)"
                },
                new RepoViewModel
                {
                    Name = "repo3",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 3, behind - 1)"
                },
            };
        }

        public List<RepoViewModel> RepoViewModels { get; set; }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            RepoViewModels = new List<RepoViewModel> {
                new RepoViewModel
                {
                    Name = "repo1",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 0, behind - 1)"
                },
                new RepoViewModel
                {
                    Name = "repo2",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 2, behind - 1)"
                },
                new RepoViewModel
                {
                    Name = "repo3",
                    CreatedAt = DateTime.Now,
                    WorkingDirectory = "C:\\Users\\Shiva\\Repos",
                    BranchNames = "Master (ahead - 3, behind - 1)"
                },

            };
        }

        private void abbtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void abbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //TODO add reponame
            AddEditRepoDialog addEditRepoDialog = new AddEditRepoDialog("Edit");
            await addEditRepoDialog.ShowAsync();
        }

        private void abbtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
