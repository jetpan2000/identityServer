using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Octacom.Odiss.Core.Contracts.Services;

namespace Octacom.Odiss.Core.Business
{
    public class EmailService : IEmailService
    {
        private readonly IConfigService configService;

        public EmailService(IConfigService configService)
        {
            this.configService = configService;
        }

        public void Send(string fromAddress, IEnumerable<string> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null)
        {
            var fromMailAddress = new MailAddress(fromAddress);
            var toMailAddresses = toAddresses.Select(address => new MailAddress(address));

            Send(fromMailAddress, toMailAddresses, subject, body, attachments, beforeSendAction);
        }

        public void Send(string toAddress, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null)
        {
            Send(new string[] { toAddress }, subject, body, attachments, beforeSendAction);
        }

        public void Send(IEnumerable<string> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null)
        {
            var toMailAddresses = toAddresses.Select(address => new MailAddress(address));

            Send(toMailAddresses, subject, body, attachments, beforeSendAction);
        }

        public void Send(IEnumerable<MailAddress> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null)
        {
            var appSettings = configService.GetApplicationSettings();

            var fromAddress = new MailAddress(configService.GetDefaultEmail(), $"Odiss - {appSettings.Name}");

            Send(fromAddress, toAddresses, subject, body, attachments, beforeSendAction);
        }

        public void Send(MailAddress fromAddress, IEnumerable<MailAddress> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null)
        {
            var message = new MailMessage
            {
                From = fromAddress,
                Subject = subject,
                Body = body
            };

            foreach (var address in toAddresses)
            {
                message.To.Add(address);
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            beforeSendAction?.Invoke(message);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
    }
}