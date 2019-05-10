using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Octacom.Odiss.Core.Identity.Constants;
using Octacom.Odiss.Core.Identity.Managers;
using IOdissLogger = Octacom.Odiss.Core.Contracts.Infrastructure.ILogger;

namespace Octacom.Odiss.Core.Identity.Middleware
{
    public class SessionMiddleware : OwinMiddleware
    {
        private readonly string redirectUrl;
        private readonly IOdissLogger odissLogger;

        public SessionMiddleware(OwinMiddleware next, string redirectUrl, IOdissLogger odissLogger) : base(next)
        {
            this.redirectUrl = redirectUrl;
            this.odissLogger = odissLogger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var user = context.Authentication.User;
            var userSessionManager = context.Get<UserSessionManager<Guid, Guid>>();

            if (user != null)
            {
                var sessionClaim = user.Claims.FirstOrDefault(x => x.Type == OdissClaims.SessionId);

                if (sessionClaim == null)
                {
                    await Next.Invoke(context);
                    return;
                }

                var sessionId = Guid.Parse(sessionClaim.Value);

                if (!await userSessionManager.IsActiveAsync(sessionId))
                {
                    await userSessionManager.RemoveAsync(sessionId);
                    context.Authentication.SignOut();

                    odissLogger.LogSystemActivity("User logged out due to session being inactive", new { userName = user.Identity.Name, sessionId, redirectUrl });
                    context.Response.Redirect(redirectUrl);
                }
                else
                {
                    await userSessionManager.ExtendAsync(sessionId);
                }
            }

            await Next.Invoke(context);
        }
    }
}
