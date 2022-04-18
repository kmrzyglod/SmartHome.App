namespace SmartHome.Clients.WebApp.Services.Shared.ApiUrlProvider;

public interface IApiUrlProviderService
{
    string GetApiUrl();
    string GetSignalRUrl();
    string GetAbsolutePathFromRelative(string relativePath, string? authToken = null);
}