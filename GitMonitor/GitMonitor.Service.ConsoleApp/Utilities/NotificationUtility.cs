using GitMonitor.DomainModel;
using GitMonitor.DomainModel.DTO;
using GitMonitor.DomainModel.Enums;
using System;
using System.Collections.Generic;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    internal class NotificationUtility
    {
        internal static void SendNotification()
        {
            try
            {
                List<Setting> settings = new Repository.SettingsRepository().GetAllSettings();

                bool isEmailNotifyEnabled = Convert.ToBoolean(settings.Find(m => m.Key == SettingEnum.EnableEmailNotifications.ToString()).Value);
                bool isDesktopNotifyEnabled = Convert.ToBoolean(settings.Find(m => m.Key == SettingEnum.EnableDesktopNotifications.ToString()).Value);

                string notificationSubject = string.Empty;
                string notificationMessage = string.Empty;
                string emailBody = StringUtility._emailTableHeader;
                string emails = string.Empty;

                //TODO: check all the settings related to smtp are available
                bool isSMTPsettingsAvailable = settings.Find(m => m.Key == SettingEnum.SMTPEmail.ToString()).Value.Length > 0 &&
                                               settings.Find(m => m.Key == SettingEnum.SMTPHost.ToString()).Value.Length > 0 &&
                                               settings.Find(m => m.Key == SettingEnum.SMTPPort.ToString()).Value.Length > 0 &&
                                               settings.Find(m => m.Key == SettingEnum.SMTPPassword.ToString()).Value.Length > 0;

                if (isEmailNotifyEnabled || isDesktopNotifyEnabled)
                {
                    List<Repo> repos = new Repository.RepoRepository().GetReposForNotifications();

                    foreach (Repo repo in repos)
                    {
                        notificationSubject = string.Format(StringUtility._notificationSubject, repo.Name);

                        foreach (Branch branch in repo.Branches)
                        {
                            //Getting status of branches
                            if (branch.AheadBy == 0 && branch.BehindBy > 0)
                            {
                                notificationMessage += string.Format(StringUtility._branchBehindByMessage, branch.Name, branch.BehindBy);
                            }
                            else if (branch.AheadBy > 0 && branch.BehindBy == 0)
                            {
                                notificationMessage += string.Format(StringUtility._branchAheadByMessage, branch.Name, branch.AheadBy);
                            }
                            else if (branch.AheadBy > 0 && branch.BehindBy > 0)
                            {
                                notificationMessage += string.Format(StringUtility._branchDivergedMessage, branch.Name, branch.AheadBy, branch.BehindBy);
                            }

                            emailBody += string.Format(StringUtility._emailTableBody, branch.Name, branch.BehindBy, branch.BehindBy);
                        }

                        emailBody += StringUtility._emailTableFooter;

                        //Sending Email Notification
                        if (isSMTPsettingsAvailable)
                        {
                            emails = new Repository.EmailGroupRepository().GetEmailGroupByIDS(repo.EmailGroupIDS);

                            EmailUtility.SendEmail(emails, notificationSubject, emailBody);

                            //Update branch's notification flag to false
                            new Repository.RepoRepository().UpdateNotiFlag(repo.RepoID, false);
                        }

                        //TODO: Sending Desktop Notification
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
