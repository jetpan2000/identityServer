using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.DataLayer.Search
{
    public class SearchEngineRegistry
    {
        private IDictionary<string, Type> mappings;

        public void RegisterMappings(IDictionary<string, Type> mappings)
        {
            this.mappings = mappings;
        }

        public Type GetEntityType(string entityName)
        {
            if (mappings == null)
            {
                throw new Exception("Mappings haven't been registered. You must call RegisterMappings in the application startup and define which Entity Names match to which types.");
            }

            if (!mappings.ContainsKey(entityName))
            {
                throw new Exception($"Missing mapping for Entity Name ${entityName}.");
            }

            return mappings[entityName];
        }
    }
}
