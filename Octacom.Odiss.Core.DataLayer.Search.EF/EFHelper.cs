using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    internal static class EFHelper
    {
        internal static IEnumerable<EntityType> GetEntityTypes(this DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            return metadata.GetItems<EntityType>(DataSpace.OSpace);
        }
    }
}
