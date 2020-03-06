using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SmartHome.Api.Bindings;
using SmartHome.Api.Filters;
using SmartHome.Infrastructure.JsonConverters;

namespace SmartHome.Api.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddAndConfigureControllers(this IServiceCollection services)
        {
            return services.AddControllers(options =>
                {
                    //Model binder used for binding from both route and body
                    var readerFactory = services.BuildServiceProvider()
                        .GetRequiredService<IHttpRequestStreamReaderFactory>();
                    options.ModelBinderProviders.Insert(0,
                        new RouteAndBodyBinderProvider(options.InputFormatters, readerFactory));
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                    //Validation handling Filter
                    options.Filters.Add<ViewModelFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                });
        }

        public static IServiceCollection ConfigureValidation(this IMvcBuilder builder, IServiceCollection services,
            Assembly assembly)
        {
            builder.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssembly(assembly); });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            return services;
        }

        public static IServiceCollection AddApiLogging(this IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddApplicationInsights());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger), typeof(Logger<ApiLog>)));
            return services;
        }
    }
}