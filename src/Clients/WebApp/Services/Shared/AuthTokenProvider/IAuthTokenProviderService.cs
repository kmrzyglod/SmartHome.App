using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.AuthTokenProvider;

public interface IAuthTokenProviderService
{
    Task<string> GetToken();
}