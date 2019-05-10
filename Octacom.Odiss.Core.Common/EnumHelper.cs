using System;

namespace Octacom.Odiss.Core.Common
{
    public static class EnumHelper
    {
        public static T ConvertTo<T>(this object value)
            where T : Enum, IConvertible
        {
            var sourceType = value.GetType();
            if (!sourceType.IsEnum)
                throw new ArgumentException("Source type is not enum");
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Destination type is not enum");
            return (T)Enum.Parse(typeof(T), value.ToString());
        }
    }
}
