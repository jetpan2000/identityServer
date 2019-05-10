using System;
using System.Runtime.Caching;
using Octacom.Odiss.Core.Contracts.Infrastructure;

namespace Octacom.Odiss.Core.Infrastructure.Odiss5
{
    public class InMemoryCache : ICachingService
    {
        public T GetOrSet<T>(string key, Func<T> setCache) where T : class
        {
            T item = MemoryCache.Default.Get(key) as T;
            if (item == null)
            {
                item = setCache();
                MemoryCache.Default.Add(key, item, ObjectCache.InfiniteAbsoluteExpiration);
            }
            return item;
        }

        public T GetOrSet<T>(string key, Func<T> setCache, TimeSpan expiryTime) where T : class
        {
            T item = MemoryCache.Default.Get(key) as T;
            if (item == null)
            {
                item = setCache();
                MemoryCache.Default.Add(key, item, DateTimeOffset.Now.Add(expiryTime));
            }
            return item;
        }

        public void Expire(string key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}
