using GitMonitor.UWP.Pages.Dialogs;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GitMonitor.UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SetTitleBar()
        {
            try
            {
                //TitleBar Customization
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                Window.Current.SetTitleBar(AppTitleBar);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void nvMainView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                nvMainView.SelectedItem = nvMainView.MenuItems[0];
                cfMainFrame.SourcePageType = typeof(DashboardPage);
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void nvMainView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            try
            {
                if (args.IsSettingsInvoked)
                {
                    cfMainFrame.Navigate(typeof(SettingsPage));
                }
                else
                {
                    // find NavigationViewItem with Content that equals InvokedItem
                    var item = sender.MenuItems
                                     .OfType<NavigationViewItem>()
                                     .First(x => (string)x.Content == (string)args.InvokedItem);

                    Navigate(item as NavigationViewItem);
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Dashboard":
                    cfMainFrame.Navigate(typeof(DashboardPage));
                    break;

                case "UntrackedRepos":
                    cfMainFrame.Navigate(typeof(UntrackedReposPage));
                    break;

                case "EmailGroups":
                    cfMainFrame.Navigate(typeof(EmailGroupsPage));
                    break;
            }
        }
    }
}