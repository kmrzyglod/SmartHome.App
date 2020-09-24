using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SmartHome.Application.Shared.Interfaces.Cache;
using SmartHome.Application.Shared.Interfaces.Query;
using System.Linq;
using SmartHome.Infrastructure.Cache;

namespace SmartHome.Api.MediatR
{
    public class RequestCacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
    {
        private readonly ICache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestCacheBehaviour(ICache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {

            string queryString = $"{_httpContextAccessor.HttpContext.Request.Path}{_httpContextAccessor.HttpContext.Request.QueryString}";
            if (_httpContextAccessor.HttpContext.Request.Method != "GET" || typeof(TRequest).GetInterfaces().Contains(typeof(INoCache)) || string.IsNullOrEmpty(queryString))
            {
                return next();
            }

            return _cache.GetOrCreateAsync(queryString, () => next(),
                new CacheOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)});
        }
    }
}
