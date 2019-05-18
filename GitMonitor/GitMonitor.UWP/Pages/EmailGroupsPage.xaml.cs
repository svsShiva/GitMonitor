﻿using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GitMonitor.UWP.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmailGroupsPage : Page
    {
        public EmailGroupsPage()
        {
            this.InitializeComponent();
        }

        public List<EmailGroup> EmailGroups { get; set; }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                EmailGroups = await APIUtility.Get<List<EmailGroup>>(RouteUtility._getGetAllEmailGroups);

                dgEmailGroups.ItemsSource = EmailGroups;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppBarButton item = (AppBarButton)sender;

                EmailGroup emailGroup = (EmailGroup)item.DataContext;

                AddEditEmailGroupDialog addEditEmailGroupDialog = new AddEditEmailGroupDialog
                    ("Edit - " + emailGroup.Name, emailGroup);

                await addEditEmailGroupDialog.ShowAsync();
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

                EmailGroup obj = ((FrameworkElement)sender).DataContext as EmailGroup;

                APIUtility.Delete(string.Format(RouteUtility._getDeleteEmailGroup, obj.EmailGroupID));

                EmailGroups.Remove(obj);

                dgEmailGroups.ItemsSource = null;
                dgEmailGroups.ItemsSource = EmailGroups;
            }
            catch (CustomExceptionUtility ex)
            {
                await new ErrorDialog(StringUtility._unexpectedError).ShowAsync();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void btnAddEmailGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEditEmailGroupDialog addEditEmailGroupDialog =
                    new AddEditEmailGroupDialog("Add Email Group", null, EmailGroups);

                await addEditEmailGroupDialog.ShowAsync();

                dgEmailGroups.ItemsSource = null;
                dgEmailGroups.ItemsSource = EmailGroups;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}
