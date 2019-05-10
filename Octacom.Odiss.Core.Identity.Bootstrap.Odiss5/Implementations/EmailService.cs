using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Entities;
using IOdissEmailService = Octacom.Odiss.Core.Contracts.Services.IEmailService;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IOdissEmailService odissEmailService;
        private readonly Core.Contracts.Services.IPrincipalService principalService;

        public EmailService(IOdissEmailService odissEmailService, Core.Contracts.Services.IPrincipalService principalService)
        {
            this.odissEmailService = odissEmailService;
            this.principalService = principalService;
        }

        public Task SendAsync(IdentityMessage message)
        {
            odissEmailService.Send(message.Destination, message.Subject, message.Body, beforeSendAction: mailMessage =>
            {
                mailMessage.IsBodyHtml = true;
            });

            return Task.FromResult(0);
        }

        public IdentityMessage GenerateForgotPasswordEmail(OdissUser user, string passwordResetKey, string callbackUrl)
        {
            callbackUrl = callbackUrl.Replace("{token}", passwordResetKey);

            var emailContent = $"Please reset your password by clicking here: <a href=\"{callbackUrl}\">link</a>";

            return new IdentityMessage
            {
                Destination = user.Email,
                Subject = "Reset Password",
                Body = GenerateEmailBody(user, emailContent)
            };
        }

        public IdentityMessage GenerateForgotUsernameEmail(OdissUser user)
        {
            return new IdentityMessage
            {
                Destination = user.Email,
                Subject = "Username Reminder",
                Body = GenerateEmailBody(user, $"Your username is {user.UserName}")
            };
        }

        public IdentityMessage GeneratePasswordChangedEmail(OdissUser user)
        {
            return new IdentityMessage
            {
                Destination = user.Email,
                Subject = "Password changed",
                Body = GenerateEmailBody(user, "Your password has been changed.")
            };
        }

        private string GenerateEmailBody(OdissUser user, string content)
        {
            var recipientName = $"{user.FirstName} {user.LastName}";
            var ipAddress = principalService.GetIpAddress();
            var ipUrl = $"https://www.ip-adress.com/ip-address/ipv4/{ipAddress}";
            string requestorIpBlurb = $"<p style=\"font-size: 13px\">This action was requested from IP address {ipAddress}. Find out more about this address <a href=\"{ipUrl}\">here</a>.<p>";

            return $"Dear {recipientName}. <br /><br />{content}<br /><br />{requestorIpBlurb}";
        }
    }
}