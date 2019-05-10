using System;
using System.Collections.Generic;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IUserDocumentRepository : IRepository<UserDocument>
    {
        IEnumerable<UserDocument> GetByUserId(Guid userId);
    }
}
