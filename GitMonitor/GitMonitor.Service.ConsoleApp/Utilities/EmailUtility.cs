using System.Net.Mail;
using System.IO;
using System.Collections.Generic;
using GitMonitor.DomainModel.DTO;
using GitMonitor.DomainModel.Enums;
using System;
using System.Net;
using GitMonitor.DomainModel;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    public class EmailUtility
    {
        public static void SendEmail(string recipients, string subject, string body, string cc = null,
                              List<Attachment> attachments = null, string folderPath = null)
        {
            List<Setting> settings = new Repository.SettingsRepository().GetAllSettings();

            using (SmtpClient smtp = new SmtpClient())
            {
                using (MailMessage message = new MailMessage())
                {
                    foreach (var item in recipients.Split(';'))
                    {
                        message.To.Add(new MailAddress(item));
                    }

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    if (cc != null)
                    {
                        foreach (var item in cc.Split(';'))
                        {
                            message.CC.Add(new MailAddress(item));
                        }
                    }

                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            message.Attachments.Add(attachment);
                        }
                    }

                    if (folderPath != null)
                    {
                        foreach (var file in Directory.GetFiles(folderPath))
                        {
                            Attachment attachment = new Attachment(file);
                            message.Attachments.Add(attachment);
                        }
                    }

                    smtp.Host = settings.Find(m => m.Key == SettingEnum.SMTPHost.ToString()).Value;
                    smtp.Port = Convert.ToInt32(settings.Find(m => m.Key == SettingEnum.SMTPPort.ToString()).Value);
                    smtp.EnableSsl = Convert.ToBoolean(settings.Find(m => m.Key == SettingEnum.SMTPEnableSsl.ToString()).Value);
                    smtp.Credentials = new NetworkCredential(settings.Find(m => m.Key == SettingEnum.SMTPEmail.ToString()).Value,
                                       EncryptDecryptUtility.Decrypt(settings.Find(m => m.Key == SettingEnum.SMTPPassword.ToString()).Value));

                    smtp.Send(message);
                }
            }
        }
    }
}