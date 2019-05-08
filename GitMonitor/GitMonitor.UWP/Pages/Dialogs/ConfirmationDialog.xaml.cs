using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class ConfirmationDialog : ContentDialog
    {
        public ConfirmationDialog(string message)
        {
            this.InitializeComponent();

            this.tbMessage.Text = message;
        }
    }
}
