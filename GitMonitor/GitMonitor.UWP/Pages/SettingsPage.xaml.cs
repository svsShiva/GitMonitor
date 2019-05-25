using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Enums;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GitMonitor.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private List<Setting> _settings { get; set; }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                _settings = await APIUtility.Get<List<Setting>>(RouteUtility._getAllSetting);

                foreach (Setting setting in _settings)
                {
                    if (setting.Key == SettingEnum.Interval.ToString())
                    {
                        tbInterval.Text = setting.Value;
                    }
                    else if (setting.Key == SettingEnum.EnableDesktopNotifications.ToString())
                    {
                        cbEnableDesktopNoti.IsChecked = Convert.ToBoolean(setting.Value);
                    }
                    else if (setting.Key == SettingEnum.EnableEmailNotifications.ToString())
                    {
                        cbEnableEmailNoti.IsChecked = Convert.ToBoolean(setting.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationDialog confirmationDialog = new ConfirmationDialog(StringUtility._saveSettingConfirmation);

                ContentDialogResult contentDialogResult = await confirmationDialog.ShowAsync();

                if (contentDialogResult == ContentDialogResult.Primary)
                {
                    APIUtility APIUtility = new APIUtility();

                    List<Setting> settings = new List<Setting>();
                    settings.Add(new Setting { Key = SettingEnum.Interval.ToString(), Value = tbInterval.Text });
                    settings.Add(new Setting { Key = SettingEnum.EnableDesktopNotifications.ToString(), Value = cbEnableDesktopNoti.IsChecked.ToString() });
                    settings.Add(new Setting { Key = SettingEnum.EnableEmailNotifications.ToString(), Value = cbEnableEmailNoti.IsChecked.ToString() });

                    await APIUtility.Put(settings, RouteUtility._updateSetting);

                    await new MessageDialog(StringUtility._settingSavedSuccessfully).ShowAsync();
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void tbInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                tbIntervalVal.Text = ValidationUtility.ValidateInterval(tbInterval.Text);

                if (tbIntervalVal.Text == string.Empty)
                {
                    int interval = Convert.ToInt16(tbInterval.Text);
                    int minVal = Convert.ToInt16(_settings.Where(m => m.Key == SettingEnum.MinInterval.ToString()).First().Value);
                    int maxVal = Convert.ToInt16(_settings.Where(m => m.Key == SettingEnum.MaxInterval.ToString()).First().Value);

                    if (interval < minVal || interval > maxVal)
                    {
                        tbIntervalVal.Text = string.Format(StringUtility._InvalidRange, minVal, maxVal);
                    }
                    else
                    {
                        btnSave.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}