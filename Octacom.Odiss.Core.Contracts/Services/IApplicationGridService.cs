using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Services
{
    public interface IApplicationGridService
    {
        TEntity MapDataToEntity<TEntity>(Guid appId, Dictionary<Guid, object> obj) where TEntity : new();
        Dictionary<Guid, object> MapEntityToData<TEntity>(Guid appId, TEntity entity);
        IEnumerable<FilterResult> ResolveFieldFilter(Guid fieldId, string value);
    }
}
