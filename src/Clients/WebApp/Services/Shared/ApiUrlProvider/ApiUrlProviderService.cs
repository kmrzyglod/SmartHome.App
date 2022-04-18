using Microsoft.Extensions.Configuration;

namespace SmartHome.Clients.WebApp.Services.Shared.ApiUrlProvider;

public class ApiUrlProviderService : IApiUrlProviderService
{
    private readonly IConfiguration _configuration;

    public ApiUrlProviderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetApiUrl()
    {
        return _configuration["ApiUrl"];
    }

    public string GetSignalRUrl()
    {
        return _configuration["SignalRUrl"];
    }

    public string GetAbsolutePathFromRelative(string relativePath, string? authToken = null)
    {
        string path = $"{GetApiUrl()}/{relativePath}";
        if (string.IsNullOrEmpty(authToken) == false) 
        {
            path = $"{path}?access_token={authToken}";
        }

        return path;
    }
}