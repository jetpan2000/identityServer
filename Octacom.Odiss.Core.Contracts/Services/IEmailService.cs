using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IEmailService
    {
        void Send(string fromAddress, IEnumerable<string> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null);
        void Send(MailAddress fromAddress, IEnumerable<MailAddress> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null);
        void Send(string toAddress, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null);
        void Send(IEnumerable<string> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null);
        void Send(IEnumerable<MailAddress> toAddresses, string subject, string body, IEnumerable<Attachment> attachments = null, Action<MailMessage> beforeSendAction = null);
    }
}
