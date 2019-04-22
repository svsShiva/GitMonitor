using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void nvMainView_Loaded(object sender, RoutedEventArgs e)
        {
            nvMainView.SelectedItem = nvMainView.MenuItems[0];
            cfMainFrame.SourcePageType = typeof(DashboardPage);
        }

        private void nvMainView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
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