using System;
using EmailService.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace EmailService.Cache
{
    public class Store
    {
        private readonly IMemoryCache cache;
        private readonly CacheConfig props;

        public Store(IMemoryCache cache, CacheConfig props)
        {
            this.cache = cache;
            this.props = props;
        }

        public T GetOrSetCache<T>(string key, Func<T> fn)
        {
            var (res, cacheHit) = GetCache(key, fn);

            if (!cacheHit)
                SetCache(key, res);

            return res;
        }

        public void SetCache<T>(string key, T res)
        {
            if (props.CacheEnabled)
            {
                string realKey = Key.Create<T>(key);

                var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(props.CacheTimespan))
                    .SetSize(props.CacheMaxSize);

                cache.Set(realKey, res, options);
            }
        }

        public (T, bool) GetCache<T>(string key, Func<T> fn)
        {
            string realKey = Key.Create<T>(key);

            if (props.CacheEnabled && cache.TryGetValue(realKey, out T cachedRes))
            {
                return (cachedRes, true);
            }

            return (fn(), false);
        }
    }
}
