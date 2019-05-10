using System;
using Octacom.Odiss.Core.Contracts.Repositories.Searching;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IUserRepository : IRepository<User>, ISearchable<User, UserSearchParameters>
    {
        User GetByEmail(string email);
        User GetByUsername(string username);
        bool EmailAlreadyRegistered(string email, Guid? currentUID);
    }
}
