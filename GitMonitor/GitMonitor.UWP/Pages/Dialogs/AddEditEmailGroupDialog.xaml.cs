using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using GitMonitor.UWP.DTO;
using Windows.UI.Xaml;
using GitMonitor.UWP.Utilities;
using System;
using System.Linq;

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class AddEditEmailGroupDialog : ContentDialog
    {
        private EmailGroup _emailGroup;

        public List<string> EmailList = new List<string>();

        public AddEditEmailGroupDialog(string title, EmailGroup emailGroup = null)
        {
            Title = title;

            if (emailGroup != null)
            {
                _emailGroup = emailGroup;
                StringToList();
            }
            else
            {
                _emailGroup = new EmailGroup();
            }

            DataContext = _emailGroup;

            this.InitializeComponent();
        }

        private void StringToList()
        {
            if (_emailGroup != null && _emailGroup.Emails != null)
            {
                EmailList = _emailGroup.Emails.Split(';').ToList<string>();
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailGroup obj = DataContext as EmailGroup;

                if (obj.Emails != null && obj.Emails.Length != 0)
                {
                    EmailList.Add(obj.Emails);

                    //refreshing Listview 
                    lvEmails.ItemsSource = null;
                    lvEmails.ItemsSource = EmailList;

                    obj.Emails = string.Empty;
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
                APIUtility APIUtility = new APIUtility();

                EmailGroup obj = DataContext as EmailGroup;

                // List of emails to comma seperated emails
                obj.Emails = null;
                foreach (string email in EmailList)
                {
                    obj.Emails += email + ";";
                }

                obj.Emails.Trim(';');

                if (_emailGroup.EmailGroupID <= 0)
                {
                    await APIUtility.Post<EmailGroup, EmailGroup>(obj, RouteUtility._getAddEmailGroup);
                }
                else
                {
                    await APIUtility.Put(obj, RouteUtility._getUpdateEmailGroup);
                }

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
                Button btn = (Button)sender;

                EmailList.Remove((string)btn.DataContext);

                //regreshing listview
                lvEmails.ItemsSource = null;
                lvEmails.ItemsSource = EmailList;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}