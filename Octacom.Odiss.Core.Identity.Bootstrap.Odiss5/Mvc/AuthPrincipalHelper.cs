using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Octacom.Odiss.Library;
using Octacom.Odiss.Library.Auth;
using Octacom.Odiss.Core.Identity.Constants;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Mvc
{
    public static class AuthPrincipalHelper
    {
        public static AuthPrincipal GetAuthPrincipalFromClaims(this HttpContextBase httpContext)
        {
            var claimsPrincipal = httpContext.User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;

            if (claimsIdentity == null)
            {
                return null;
            }

            var authPrincipal = new AuthPrincipal(claimsIdentity.Name);

            Action<string, Action<string>> setClaim = (claimType, setter) =>
            {
                var claim = claimsIdentity.FindFirst(claimType);

                if (claim != null)
                {
                    setter(claim.Value);
                }
            };

            setClaim(OdissClaims.Id, value => { authPrincipal.ID = Guid.Parse(value); });
            setClaim(OdissClaims.UserName, value => { authPrincipal.UserName = value; });
            setClaim(OdissClaims.UserType, value => { authPrincipal.UserType = (UserTypeEnum)Convert.ToInt32(value); });
            setClaim(OdissClaims.Permissions, value => { authPrincipal.Permissions = (UserPermissionsEnum)Convert.ToInt32(value); });
            setClaim(OdissClaims.Applications, value => { authPrincipal.Applications = SplitGuids(value); });
            setClaim(OdissClaims.Groups, value => { authPrincipal.Groups = SplitGuids(value); });
            setClaim(OdissClaims.Email, value => { authPrincipal.Email = value; });
            setClaim(OdissClaims.FirstName, value => { authPrincipal.FirstName = value; });
            setClaim(OdissClaims.LastName, value => { authPrincipal.LastName = value; });

            return authPrincipal;
        }

        private static Guid[] SplitGuids(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new Guid[0];
            }

            return value.Split(',').Select(stringGuid => Guid.Parse(stringGuid)).ToArray();
        }
    }
}
