using System;
using System.Linq;
using System.Security.Claims;
using Octacom.Odiss.Library;
using Octacom.Odiss.Library.Auth;
using Octacom.Odiss.Core.Identity.Constants;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Mvc
{
    public class BaseController : Octacom.Odiss.Library.BaseController
    {
        protected override AuthPrincipal User
        {
            get
            {
                return HttpContext.GetAuthPrincipalFromClaims();
            }
        }
    }
}
