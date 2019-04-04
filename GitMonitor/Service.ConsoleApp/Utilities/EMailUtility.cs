using System;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Collections.Generic;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    public class EmailUtility
    {
        public bool SendEmail(string recipients, string subject, string body, string cc,
                              List<Attachment> attachments = null, string folderPath = null)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage message = new MailMessage())
                    {
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsProduction"]))
                        {
                            foreach (var item in recipients.Split(';'))
                            {
                                message.To.Add(new MailAddress(item));
                            }
                        }
                        else
                        {
                            var tempRecipients = ConfigurationManager.AppSettings["TestMail"];
                            foreach (var item in tempRecipients.Split(';'))
                            {
                                message.To.Add(new MailAddress(item));
                            }
                        }

                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        if (!cc.Equals(""))
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

                        smtp.Send(message);

                        //return ValidationEnum.Success;
                        return true;
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