using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    public interface IOdissUserStore : IUserLockoutStore<OdissUser, Guid>, IUserPasswordStore<OdissUser, Guid>, IUserTwoFactorStore<OdissUser, Guid>, IUserRoleStore<OdissUser, Guid>, IUserEmailStore<OdissUser, Guid>, IUserClaimStore<OdissUser, Guid>
    {
        Task SetClaimsAsync(OdissUser user, IList<Claim> claims);
    }
}
