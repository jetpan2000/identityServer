using System;
using Octacom.Odiss.Core.Contracts.Infrastructure;

namespace Octacom.Odiss.Core.IntegrationTests
{
    public class CachingServiceMock : ICachingService
    {
        public void Expire(string key)
        {
            // Do nothing
        }

        public T GetOrSet<T>(string key, Func<T> setCache) where T : class
        {
            // Do not cache anything, only pretend that we did :-) (for testing only)
            return setCache();
        }

        public T GetOrSet<T>(string key, Func<T> setCache, TimeSpan expiryTime) where T : class
        {
            // Do not cache anything, only pretend that we did :-) (for testing only)
            return setCache();
        }
    }
}
