using System.Security.Principal;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IPrincipalService
    {
        IPrincipal GetCurrentPrincipal();
        string GetIpAddress();
    }
}
