using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.ApiClient
{
    public interface IApiClient
    {
        IDictionary<string, IEnumerable<string>>? NoCacheHeader { get; }
        Task<TResponse?> Get<TResponse>(string url, IDictionary<string, IEnumerable<string>>? customHeaders = null)
            where TResponse : class;

        Task<TResponse?> Get<TQuery, TResponse>(string url, TQuery query,
            IDictionary<string, IEnumerable<string>>? customHeaders = null)
            where TQuery : class where TResponse : class;

        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
        Task Post<TRequest>(string url, TRequest request) where TRequest : class;
        Task<TResponse> Put<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
        Task Put<TRequest>(string url, TRequest request) where TRequest : class;
        Task<TResponse> Patch<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
        Task Patch<TRequest>(string url, TRequest request) where TRequest : class;
        Task Delete(string url);
        Task Delete<TRequest>(string url, TRequest query) where TRequest : class;
        Task<TResponse> Delete<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
    }
}