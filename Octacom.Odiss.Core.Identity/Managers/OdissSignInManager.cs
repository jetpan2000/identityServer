using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Managers
{
    public class OdissSignInManager : SignInManager<OdissUser, Guid>
    {
        private readonly IAuthenticationAdapter authenticationAdapter;
        private readonly UserSessionManager<Guid, Guid> userSessionManager;
        private readonly ILogger logger;

        public OdissSignInManager(UserManager<OdissUser, Guid> userManager, IAuthenticationManager authenticationManager, IAuthenticationAdapter authenticationAdapter, UserSessionManager<Guid, Guid> userSessionManager, ILogger logger) : base(userManager, authenticationManager)
        {
            this.authenticationAdapter = authenticationAdapter;
            this.userSessionManager = userSessionManager;
            this.logger = logger;
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (authenticationAdapter.IsAuthenticatingInLegacyOdiss(userName, password))
            {
                var user = await UserManager.FindByNameAsync(userName);
                var passwordResetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                await UserManager.ResetPasswordAsync(user.Id, passwordResetToken, password);
            }

            var result = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            logger.LogActivity("PasswordSignIn", new { userName, result });
            return result;
        }

        public async Task SignOutAsync()
        {
            var user = AuthenticationManager.User;

            if (user != null)
            {
                var sessionClaim = user.Claims.FirstOrDefault(x => x.Type == OdissClaims.SessionId);

                if (sessionClaim != null)
                {
                    var sessionId = Guid.Parse(sessionClaim.Value);
                    await userSessionManager.RemoveAsync(sessionId);
                }
            }

            logger.LogActivity("SignOut", new { userName = user.Identity.Name });

            AuthenticationManager.SignOut();
        }
    }
}