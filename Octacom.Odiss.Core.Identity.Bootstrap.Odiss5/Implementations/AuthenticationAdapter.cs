using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Library.Auth;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Implementations
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {
        public bool IsAuthenticatingInLegacyOdiss(string username, string password)
        {
            AuthLogin login = new AuthLogin();
            return login.ValidateUser(username, password);
        }
    }
}