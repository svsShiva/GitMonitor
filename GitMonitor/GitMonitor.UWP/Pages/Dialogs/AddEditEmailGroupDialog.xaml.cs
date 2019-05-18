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
        private bool _iSNameValid = false;
        private bool _iSEmailValid = false;
        private List<EmailGroup> _emailGroups;

        public List<string> EmailList = new List<string>();

        public AddEditEmailGroupDialog(string title, EmailGroup emailGroup = null, List<EmailGroup> emailGroups = null)
        {
            try
            {
                Title = title;

                if (emailGroup != null)
                {
                    _emailGroup = emailGroup;
                    StringToList();
                    //_emailGroup.Emails = string.Empty;
                    _iSNameValid = true;
                    _iSEmailValid = true;
                    EnablePrimaryButton();
                }
                else
                {
                    _emailGroup = new EmailGroup();
                    IsPrimaryButtonEnabled = false;
                    _emailGroups = emailGroups;
                }

                DataContext = _emailGroup;

                this.InitializeComponent();
            }
            catch (Exception ex)
            {
                new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EmailList.Contains(tbEmail.Text))
                {
                    tbEmailVal.Text = StringUtility._emailAlreadyAdded;
                    return;
                }

                string emaliVal = ValidationUtility.ValidateEmail(tbEmail.Text);
                _iSEmailValid = emaliVal != string.Empty ? false : true;
                tbEmailVal.Text = emaliVal;

                if (_iSEmailValid)
                {
                    EmailGroup obj = DataContext as EmailGroup;

                    if (obj.Emails != null)
                    {
                        EmailList.Add(tbEmail.Text);

                        //refreshing Listview 
                        lvEmails.ItemsSource = null;
                        lvEmails.ItemsSource = EmailList;

                        tbEmail.Text = string.Empty;
                    }

                    EnablePrimaryButton();
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

                obj.Emails = obj.Emails.Trim(';');

                if (_emailGroup.EmailGroupID <= 0)
                {
                    EmailGroup emailGroup = await APIUtility.Post<EmailGroup, EmailGroup>(obj, RouteUtility._getAddEmailGroup);
                    _emailGroups.Add(emailGroup);
                }
                else
                {
                    await APIUtility.Put(obj, RouteUtility._getUpdateEmailGroup);
                    obj.LastModifiedAt = DateTime.Now;
                }
            }
            catch (CustomExceptionUtility ex)
            {
                if (ex.Message == StringUtility._nameExists)
                {
                    await new ErrorDialog(StringUtility._nameExists).ShowAsync();
                }
                else
                {
                    await new ErrorDialog(StringUtility._unexpectedError).ShowAsync();
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

                EnablePrimaryButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
        private async void tbGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string nameVal = ValidationUtility.ValidateName(_emailGroup.Name);
                _iSNameValid = nameVal != string.Empty ? false : true;
                tbGroupNameVal.Text = nameVal;

                EnablePrimaryButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
        private async void tbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tbEmailVal.Text = string.Empty;

                if (tbEmail.Text.Length > 0)
                {
                    tbEmailVal.Text = ValidationUtility.ValidateEmail(tbEmail.Text);
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }



        private void EnablePrimaryButton(bool isFromAddbtn = false)
        {
            this.IsPrimaryButtonEnabled = _iSNameValid && EmailList.Count > 0;
        }

        private void StringToList()
        {
            if (_emailGroup != null && _emailGroup.Emails != null)
            {
                EmailList = _emailGroup.Emails.Split(';').ToList<string>();
            }
        }
    }
}