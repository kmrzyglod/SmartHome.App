using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.ApiClient
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(string url);
        Task<TResponse> Get<TQuery, TResponse>(string url, TQuery query) where TQuery : class;
        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
        Task Post<TRequest>(string url, TRequest request) where TRequest : class;
        Task<TResponse> Put<TRequest, TResponse>(string url, TRequest request) where TRequest : class;
        Task Put<TRequest>(string url, TRequest request) where TRequest : class;
        Task Delete(string url);
        Task Delete<TQuery>(string url, TQuery query) where TQuery : class;
    }
}