using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.DataLayer
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
