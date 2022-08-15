using Bytes2you.Validation;
using Microsoft.Extensions.Caching.Memory;
using System;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;
        public CacheProvider(IMemoryCache memoryCache)
        {
            Guard.WhenArgument<IMemoryCache>(memoryCache, GlobalConstants.GetMemberName(() => memoryCache)).IsNull().Throw();

            this.memoryCache = memoryCache;
        }
        public T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds)
        {
            T data;
            if (!memoryCache.TryGetValue(itemName, out data))
            {
                data = getDataFunc();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(durationInSeconds))
                    .SetSize(1024)
                    .SetPriority(CacheItemPriority.Normal);

                // Save data in cache.
                memoryCache.Set(itemName, data, cacheEntryOptions);
            }

            return data;
        }

        public void Remove(string itemName)
        {
            memoryCache.Remove(itemName);
        }
    }
}
