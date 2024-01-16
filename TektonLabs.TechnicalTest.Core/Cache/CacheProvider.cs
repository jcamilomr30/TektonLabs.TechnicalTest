using Microsoft.Extensions.Caching.Memory;
using System;
using TektonLabs.TechnicalTest.Core.Interfaces;

namespace TektonLabs.TechnicalTest.Core.Cache
{
    public class CacheProvider : ICacheProvider
    {
        private static IMemoryCache _cache;

        static CacheProvider()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public TItem Set<TItem>(string key, TItem value, int expiration)
        {
            return _cache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiration)
            });
        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
