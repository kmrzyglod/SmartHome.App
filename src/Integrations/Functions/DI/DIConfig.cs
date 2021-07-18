using Microsoft.Extensions.DependencyInjection;
using SmartHome.Integrations.Functions.ScheduleTasks.Infrastructure.ScheduledTasksProvider;

namespace SmartHome.Integrations.Functions.DI
{
    public static class DIConfig
    {
        public static IServiceCollection AddScheduledTasksProvider(this IServiceCollection services)
        {
            services.AddSingleton<IScheduledTasksProvider, ScheduledTasksProvider>();
            return services;
        }
    }
}