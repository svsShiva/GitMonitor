using GitMonitor.UWP.Utilities;
using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages.Dialogs
{
    public sealed partial class ErrorDialog : ContentDialog
    {

        public ErrorDialog(Exception ex, bool isGenericMessage = true)
        {
            this.InitializeComponent();
            tbMessage.Text = StringUtility._unexpectedError;

            //TODO
            //Log exception make an AICALL
            //Need to use local log utillity -- Filesystem

            if (!isGenericMessage)
            {
                tbMessage.Text = ex.Message;
            }
        }

        public ErrorDialog(string message)
        {
            this.InitializeComponent();
            tbMessage.Text = message;
        }
    }
}
