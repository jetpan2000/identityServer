using Microsoft.AspNet.Identity;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    /// <summary>
    /// Interface with SMS messaging services. The purpose of this is to distinguish between e-mails and SMS as they use the same interface.
    /// We need to have the ability to refer to these by a unique interface type.
    /// </summary>
    public interface ISmsService : IIdentityMessageService
    {
    }
}
