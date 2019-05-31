using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Enums;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GitMonitor.UWP.Pages
{
    public sealed partial class SettingsPage : Page
    {
        private bool _isEmailValid = false;
        private bool _isIntervalValid = false;
        private List<Setting> _settings { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();
        }

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
                    else if (setting.Key == SettingEnum.SMTPEmail.ToString())
                    {
                        tbSMTPEmail.Text = setting.Value;
                    }
                    else if (setting.Key == SettingEnum.SMTPHost.ToString())
                    {
                        tbSMTPHost.Text = setting.Value;
                    }
                    else if (setting.Key == SettingEnum.SMTPPort.ToString())
                    {
                        tbSMTPPort.Text = setting.Value;
                    }
                    else if (setting.Key == SettingEnum.SMTPEnableSsl.ToString())
                    {
                        cbSMTPEnableSsl.IsChecked = Convert.ToBoolean(setting.Value);
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

                    string password = string.Empty;
                    if (pbSMTPPassword.Password != StringUtility._proxyPassword)
                    {
                        password = pbSMTPPassword.Password;
                    }

                    List<Setting> settings = new List<Setting>();
                    settings.Add(new Setting { Key = SettingEnum.Interval.ToString(), Value = tbInterval.Text });
                    settings.Add(new Setting { Key = SettingEnum.EnableDesktopNotifications.ToString(), Value = cbEnableDesktopNoti.IsChecked.ToString() });
                    settings.Add(new Setting { Key = SettingEnum.EnableEmailNotifications.ToString(), Value = cbEnableEmailNoti.IsChecked.ToString() });
                    settings.Add(new Setting { Key = SettingEnum.SMTPEmail.ToString(), Value = tbSMTPEmail.Text });
                    settings.Add(new Setting { Key = SettingEnum.SMTPPassword.ToString(), Value = password });
                    settings.Add(new Setting { Key = SettingEnum.SMTPHost.ToString(), Value = tbSMTPHost.Text });
                    settings.Add(new Setting { Key = SettingEnum.SMTPPort.ToString(), Value = tbSMTPPort.Text });
                    settings.Add(new Setting { Key = SettingEnum.SMTPEnableSsl.ToString(), Value = cbSMTPEnableSsl.IsChecked.ToString() });

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
                _isIntervalValid = false;
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
                        _isIntervalValid = true;
                    }
                }
                EnableSaveButton();
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
                _isEmailValid = false;

                tbEmailVal.Text = ValidationUtility.ValidateEmail(tbSMTPEmail.Text);

                _isEmailValid = tbEmailVal.Text == string.Empty ? true : false;

                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
        private async void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                tbPasswordVal.Text = pbSMTPPassword.Password.Length > 0 ? string.Empty : StringUtility._emptyField;

                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void cbEnableEmailNoti_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void cbEnableEmailNoti_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void tbSMTPHost_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tbSMTPHostVal.Text = tbSMTPHost.Text.Length > 0 ? string.Empty : StringUtility._emptyField;

                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void tbSMTPPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tbSMTPPortVal.Text = tbSMTPPort.Text.Length > 0 ? string.Empty : StringUtility._emptyField;

                EnableSaveButton();
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private void EnableSaveButton()
        {
            btnSave.IsEnabled = Convert.ToBoolean(cbEnableEmailNoti.IsChecked) == true ?
                                 _isEmailValid && _isIntervalValid && pbSMTPPassword.Password.Length > 0 && 
                                 tbSMTPHost.Text.Length > 0 && tbSMTPPort.Text.Length > 0 :
                                 _isIntervalValid;
        }
    }
}