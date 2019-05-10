using Newtonsoft.Json;

namespace Octacom.Odiss.Core.Common
{
    public static class Json
    {
        public static string SerializeJSON<T>(this T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }

        public static T DeserializeJSON<T>(this string jsonString)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
