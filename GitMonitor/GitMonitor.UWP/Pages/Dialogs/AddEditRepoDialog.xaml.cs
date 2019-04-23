using GitMonitor.UWP.DTO;
using GitMonitor.UWP.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class AddEditRepoDialog : ContentDialog
    {
        public AddEditRepoDialog(string title)
        {
            this.Title = title;

            this.InitializeComponent();
        }

        public AddEditViewModel AddEditViewModel = new AddEditViewModel
        {
            RepoName = "Rep Name",
            RepoURL = "URL",
            WorkingDirectory = "c:\\Shiva\\...",
            Branches = new List<Branch>
                   {
                        new Branch{ Name="one", AutoPull = true},
                        new Branch{ Name="two", AutoPull = false},
                        new Branch{ Name="three", AutoPull = true}
                   },
            EnableDesktopNotification = true,
            BranchesToNotify = new List<Branch>
                   {
                        new Branch{ Name="one",  EnableDeskTopNotifications = true},
                        new Branch{ Name="three", EnableDeskTopNotifications = false}
                   },
        };

        private void ContentDialog_SaveClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void hlRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
