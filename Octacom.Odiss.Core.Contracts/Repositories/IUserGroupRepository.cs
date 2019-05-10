using System;
using System.Collections.Generic;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        IEnumerable<UserGroup> GetByUserId(Guid userId);
    }
}