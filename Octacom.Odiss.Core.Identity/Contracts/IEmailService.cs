using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    /// <summary>
    /// Interface with Email messaging services. The purpose of this is to distinguish between e-mails and SMS as they use the same interface.
    /// We need to have the ability to refer to these by a unique interface type.
    /// </summary>
    public interface IEmailService : IIdentityMessageService
    {
        IdentityMessage GenerateForgotPasswordEmail(OdissUser user, string passwordResetKey, string callbackUrl);
        IdentityMessage GeneratePasswordChangedEmail(OdissUser user);
        IdentityMessage GenerateForgotUsernameEmail(OdissUser user);
    }
}
