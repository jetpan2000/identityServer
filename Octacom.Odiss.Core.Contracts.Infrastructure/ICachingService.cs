using System;

namespace Octacom.Odiss.Core.Contracts.Infrastructure
{
    public interface ICachingService
    {
        T GetOrSet<T>(string key, Func<T> setCache) where T : class;
        T GetOrSet<T>(string key, Func<T> setCache, TimeSpan expiryTime) where T : class;
        void Expire(string key);
    }
}
