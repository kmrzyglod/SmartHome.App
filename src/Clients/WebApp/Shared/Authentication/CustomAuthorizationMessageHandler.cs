using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SmartHome.Clients.WebApp.Services.Shared.ApiUrlProvider;

namespace SmartHome.Clients.WebApp.Shared.Authentication
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, 
            NavigationManager navigationManager, IApiUrlProviderService apiUrlProvider)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { apiUrlProvider.GetApiUrl(), apiUrlProvider.GetSignalRUrl() });
        }
    }
}
