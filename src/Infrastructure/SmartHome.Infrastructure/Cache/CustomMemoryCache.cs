using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SmartHome.Application.Shared.Interfaces.Cache;

namespace SmartHome.Infrastructure.Cache
{
    public class CustomMemoryCache : ICache
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> LockingDictionary = new ConcurrentDictionary<string, SemaphoreSlim>();
        private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
        private readonly CacheOptions _cacheEntryOptions = new CacheOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)};

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
        {
            return await GetOrCreateAsync(key, factory, _cacheEntryOptions);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, ICacheOptions cacheOptions)
        {
            return await GetOrCreateAsync(key, factory, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow
            });
        }

        public T GetOrCreate<T>(string key, Func<T> factory, ICacheOptions cacheOptions)
        {
            return GetOrCreate(key, factory, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow
            });
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        private async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, MemoryCacheEntryOptions memoryCacheEntryOptions)
        {
            if (Cache.TryGetValue<T>(key, out var result))
            {
                return result;
            }

            var semaphoreSlim = LockingDictionary.GetOrAdd(key, new SemaphoreSlim(1, 1));
            await semaphoreSlim.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                if (Cache.TryGetValue(key, out result))
                {
                    return result;
                }
                else
                {
                    result = await factory()
                        .ConfigureAwait(false);

                    return Cache.Set(key, result, memoryCacheEntryOptions);
                }
            }            
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public T GetOrCreate<T>(string key, Func<T> factory)
        {
            return GetOrCreate(key, factory, _cacheEntryOptions);
        }

        private T GetOrCreate<T>(string key, Func<T> factory, MemoryCacheEntryOptions memoryCacheEntryOptions)
        {
            if (Cache.TryGetValue<T>(key, out var result))
            {
                return result;
            }

            var semaphoreSlim = LockingDictionary.GetOrAdd(key, new SemaphoreSlim(1, 1));
            semaphoreSlim.Wait();
            try
            {
                if (Cache.TryGetValue(key, out result))
                {
                    return result;
                }
                else
                {
                    result = factory();
                    return Cache.Set(key, result, memoryCacheEntryOptions);
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}