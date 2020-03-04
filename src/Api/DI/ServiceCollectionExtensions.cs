using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Api.Bindings;
using SmartHome.Api.Filters;

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

                    //Validation handling Filter
                    options.Filters.Add<ViewModelFilter>();
                })
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        public static IServiceCollection ConfigureValidation(this IMvcBuilder builder, IServiceCollection services,
            Assembly assembly)
        {
            builder.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssembly(assembly); });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            return services;
        }
    }
}