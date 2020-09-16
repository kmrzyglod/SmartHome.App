using System;
using System.Threading.Tasks;

namespace SmartHome.Application.Shared.Interfaces.Cache
{
    public interface ICache
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory);

        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, ICacheOptions cacheOptions);

        T GetOrCreate<T>(string key, Func<T> factory, ICacheOptions cacheOptions);
        void Remove(string key);
    }
}