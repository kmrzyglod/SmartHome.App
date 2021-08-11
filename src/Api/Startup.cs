using System.Reflection;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SmartHome.Api.DI;
using SmartHome.Api.MediatR;
using SmartHome.Api.Middleware;
using SmartHome.Api.Swagger;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Infrastructure.DI;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartHome.Api
{
    public class Startup
    {
        private static readonly Assembly _applicationSharedAssembly = typeof(ICommand).Assembly;
        private static readonly Assembly _applicationAssembly = typeof(IApplicationDbContext).Assembly;

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddAndConfigureControllers()
                .ConfigureValidation(services, _applicationSharedAssembly);

            services.AddFramework()
                .AddCors()
                .AddApiLogging()
                .AddHealthCheckService()
                .AddConfiguration()
                .AddNotificationService()
                .AddEventStoreClient()
                .AddDeviceCommandBus()
                .AddApplicationDatabase()
                .AddEmailSender()
                .AddEventGridMessageHandling()
                .AddCommandBus()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestCacheBehaviour<,>))
                .InitMediatR(_applicationSharedAssembly, _applicationAssembly)
                .AddApiVersioning(options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                })
                .AddVersionedApiExplorer(
                    options =>
                    {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    })
                .AddSwaggerGen(
                    options =>
                    {
                        // add a custom operation filter which sets default values
                        options.OperationFilter<SwaggerDefaultValues>();
                        options.CustomSchemaIds(x => x.FullName);
                    })
                .AddFluentValidationRulesToSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());
            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}