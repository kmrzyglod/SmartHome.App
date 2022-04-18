using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Polly;

namespace SmartHome.Clients.WebApp.Services.Shared.AuthTokenProvider;

public class AuthTokenProviderService : IAuthTokenProviderService
{
    private readonly IConfiguration _configuration;
    private readonly IAccessTokenProvider _tokenProvider;

    public AuthTokenProviderService(IAccessTokenProvider tokenProvider, IConfiguration configuration)
    {
        _tokenProvider = tokenProvider;
        _configuration = configuration;
    }

    public Task<string> GetToken()
    {
        IEnumerable<string> defaultScopes = new List<string>();
        _configuration.Bind("DefaultAccessTokenScopes", defaultScopes);

        return Policy.Handle<Exception>()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryAttempt, 2)))
            .ExecuteAsync(async () =>
            {
                var tokenResult = await _tokenProvider.RequestAccessToken(new AccessTokenRequestOptions
                {
                    Scopes = defaultScopes
                });
                if (tokenResult.TryGetToken(out var token))
                {
                    return token.Value;
                }

                throw new Exception("Can't load token");
            });
    }
}