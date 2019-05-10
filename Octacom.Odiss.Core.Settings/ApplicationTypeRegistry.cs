using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Settings
{
    public class ApplicationTypeRegistry
    {
        private IDictionary<Type, string> mappings;

        public void RegisterMappings(IDictionary<Type, string> mappings)
        {
            this.mappings = mappings;
        }

        public string GetIdentifier(Type type)
        {
            if (mappings == null)
            {
                throw new Exception("Mappings haven't been registered. You must call RegisterMappings in the application startup and define which Type match to which application identifiers.");
            }

            if (!mappings.ContainsKey(type))
            {
                throw new Exception($"Missing mapping for Type ${type}.");
            }

            return mappings[type];
        }

        public bool HasTypeRegistered(Type type)
        {
            return mappings.ContainsKey(type);
        }
    }
}
