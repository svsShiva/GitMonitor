using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class MessageDialog : ContentDialog
    {
        public MessageDialog(string message, string title = "GitMonitor")
        {
            this.InitializeComponent();

            this.Title = title;
            this.tbMessage.Text = message;
        }
    }
}
