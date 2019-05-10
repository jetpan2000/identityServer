using System;
using System.Collections.Generic;
using Octacom.Odiss.Core.Entities.Application;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IApplicationRepository : IRepository<Application>
    {
        IEnumerable<Application> GetByUserId(Guid userId);
    }
}
