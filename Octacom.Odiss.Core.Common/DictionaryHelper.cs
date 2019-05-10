using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.Common
{
    public static class DictionaryHelper
    {
        public static TEntity ToEntity<TEntity>(this Dictionary<string, object> dictionary, Dictionary<string, Func<object, object>> customResolvers = null)
            where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var type = typeof(TEntity);

            foreach (var kvp in dictionary)
            {
                try
                {
                    var property = type.GetProperty(kvp.Key);

                    if (!property.CanWrite)
                    {
                        continue;
                    }

                    object value = customResolvers != null && customResolvers.ContainsKey(kvp.Key)
                        ? customResolvers[kvp.Key](kvp.Value)
                        : kvp.Value;

                    if (property.PropertyType != typeof(string) && !customResolvers.ContainsKey(kvp.Key))
                    {
                        value = Convert.ChangeType(value, property.PropertyType);
                    }
                    
                    property.SetValue(entity, value);
                }
                catch { }
            }

            return entity;
        }
    }
}
