using System;
using System.Linq;

namespace Octacom.Odiss.Core.Odiss5Adapters
{
    internal static class ReflectionHelper
    {
        internal static T GetPropertyValue<T>(object dynamicObject, string propertyName)
        {
            var type = dynamicObject.GetType();
            var valueProperty = type.GetProperties().FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (valueProperty == null)
            {
                return default(T);
            }

            return (T)valueProperty.GetValue(dynamicObject, null);
        }

        internal static string GetPropertyStringValue(object dynamicObject, string propertyName)
        {
            var type = dynamicObject.GetType();
            var valueProperty = type.GetProperties().FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (valueProperty == null)
            {
                return null;
            }

            var returnValue = valueProperty.GetValue(dynamicObject, null);

            return returnValue.ToString();
        }
    }
}