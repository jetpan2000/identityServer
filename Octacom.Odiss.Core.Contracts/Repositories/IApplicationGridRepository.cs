using Octacom.Odiss.Core.Contracts.Repositories.Searching;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface IApplicationGridRepository
    {
        IEnumerable<dynamic> GetAll(Guid appId);
        dynamic Insert(Guid appId, Dictionary<Guid, object> obj);
        void Update(Guid appId, Dictionary<Guid, object> obj);
        void Delete(Guid appId, object recordKey);
        dynamic Search(Guid appId, SearchOptions searchOptions);
        void SetMappings(IDictionary<Guid, Type> mappings);
    }
}
