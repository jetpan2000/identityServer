using Octacom.Odiss.Core.Entities.Application;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IFieldRepository : IRepository<Field>
    {
        IEnumerable<Field> GetFieldsByApplication(Guid applicationId);
    }
}
