using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SmartHome.Application.Shared.Interfaces.Cache;
using SmartHome.Application.Shared.Interfaces.Query;
using SmartHome.Infrastructure.Cache;

namespace SmartHome.Api.MediatR
{
    public class RequestCacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
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
            string queryString =
                $"{_httpContextAccessor.HttpContext?.Request.Path}{_httpContextAccessor.HttpContext?.Request.QueryString}";
            if (_httpContextAccessor.HttpContext?.Request.Method != "GET" ||
                typeof(TRequest).GetInterfaces().Contains(typeof(INoCache)) || string.IsNullOrEmpty(queryString) ||
                IsNoCacheHeaderSent())
            {
                return next();
            }

            return _cache.GetOrCreateAsync(queryString, () => next(),
                new CacheOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)});
        }

        private bool IsNoCacheHeaderSent()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers.Contains(
                new KeyValuePair<string, StringValues>("Cache-Control", new StringValues("no-cache"))) ?? false;
        }
    }
}