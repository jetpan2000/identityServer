using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Identity.Contracts
{
    public interface IOdissUserRepository
    {
        IEnumerable<Guid> GetUserApplications(Guid userId);
        IEnumerable<Guid> GetUserGroups(Guid userId);
    }
}
