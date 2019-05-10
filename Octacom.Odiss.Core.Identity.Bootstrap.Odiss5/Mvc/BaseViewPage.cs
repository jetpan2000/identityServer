using Octacom.Odiss.Library.Auth;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Mvc
{
    public abstract class BaseViewPage : Octacom.Odiss.Library.BaseViewPage
    {
        public override AuthPrincipal User
        {
            get
            {
                return Context.GetAuthPrincipalFromClaims();
            }
        }
    }

    public abstract class BaseViewPage<TModel> : Octacom.Odiss.Library.BaseViewPage<TModel>
    {
        public override AuthPrincipal User
        {
            get
            {
                return Context.GetAuthPrincipalFromClaims();
            }
        }
    }
}
