using System.Collections.Generic;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Clones the content of the dictionary and returns another one that is identical but of a different reference. The cloning is shallow (child elements are by same reference if reference type).
        /// </summary>
        /// <param name="source">Source dictionary</param>
        /// <returns>Clone dictionary of the source</returns>
        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(source.Count);

            foreach (KeyValuePair<TKey, TValue> entry in source)
            {
                ret.Add(entry.Key, entry.Value);
            }

            return ret;
        }
    }
}