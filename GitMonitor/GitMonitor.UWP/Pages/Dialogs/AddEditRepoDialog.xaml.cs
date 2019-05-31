using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using GitMonitor.UWP.DTO;
using Windows.UI.Xaml;
using GitMonitor.UWP.Utilities;
using System;
using System.Linq;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class AddEditRepoDialog : ContentDialog
    {
        public List<EmailGroup> EmailGroups;

        private List<string> _emailGroups;

        private string _repoName = string.Empty;

        public AddEditRepoDialog(string title, Repo repo = null)
        {
            this.Title = title;

            if (repo != null)
            {
                DataContext = repo;

                _emailGroups = repo.EmailGroupIDS != null ?
                                    repo.EmailGroupIDS.Split(';').ToList() :
                                    new List<string>();

                _repoName = repo.Name;
            }

            GetEmailGroups();

            this.InitializeComponent();
        }

        private async void GetEmailGroups()
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                EmailGroups = await APIUtility.Get<List<EmailGroup>>(RouteUtility._getAllEmailGroups);

                foreach (EmailGroup emailGroup in EmailGroups)
                {
                    if (_emailGroups.Contains(emailGroup.EmailGroupID.ToString()))
                    {
                        emailGroup.IsSelected = true;
                    }
                }
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

                repo.EmailGroupIDS = string.Empty;

                foreach (EmailGroup emailGroup in EmailGroups)
                {
                    if (emailGroup.IsSelected == true)
                    {
                        repo.EmailGroupIDS += emailGroup.EmailGroupID + ";";
                    }
                }

                repo.EmailGroupIDS = repo.EmailGroupIDS.TrimEnd(';');

                APIUtility APIUtility = new APIUtility();

                await APIUtility.Put(repo, RouteUtility._updateRepo);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void ContentDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Repo repo = DataContext as Repo;

            repo.Name = _repoName;
        }

        private async void hlRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repo repo = DataContext as Repo;

                APIUtility APIUtility = new APIUtility();

                DataContext = await APIUtility.Get<Repo>(string.Format(RouteUtility._refreshRepo, repo.RepoID));
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void tbRepoName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbRepoNameVal.Text = string.Empty;

            if (tbRepoName.Text.Length == 0)
            {
                tbRepoNameVal.Text = StringUtility._emptyField;
                this.IsPrimaryButtonEnabled = false;
            }
            else
            {
                this.IsPrimaryButtonEnabled = true;
            }
        }
    }
}
