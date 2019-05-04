using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using GitMonitor.UWP.DTO;
using Windows.UI.Xaml;
using GitMonitor.UWP.Utilities;
using System;
using System.Threading.Tasks;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class AddEditRepoDialog : ContentDialog
    {
        public List<EmailGroup> EmailGroups;

        public AddEditRepoDialog(string title, Repo repo = null)
        {
            this.Title = title;

            if (repo != null)
            {
                DataContext = repo;
            }
           
            this.InitializeComponent();
        }

        private async void CdAddEditRepo_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                EmailGroups = await APIUtility.Get<List<EmailGroup>>(RouteUtility._getGetAllEmailGroups);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void ContentDialog_SaveClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                Repo repo = DataContext as Repo;

                foreach (EmailGroup emailGroup in EmailGroups)
                {
                    if (emailGroup.IsSelected == true)
                    {
                        repo.EmailGroupIDS += emailGroup.Emails;
                    }
                }

                APIUtility APIUtility = new APIUtility();

               // APIUtility.Put(repo, RouteUtility._getUpdateRepo);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void ContentDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private async void hlRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo repo = DataContext as Repo;

                APIUtility APIUtility = new APIUtility();

                DataContext = await APIUtility.Get<Repo>(string.Format(RouteUtility._getRefreshRepo, repo.RepoID));
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void AbbtnAddEmailGroup_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
