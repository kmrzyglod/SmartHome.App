using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Logging;
using SmartHome.Application.Shared.Helpers;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Integrations.Functions.ScheduleTasks.Infrastructure.ScheduledTasksProvider
{
    public class ScheduledTasksProvider: IScheduledTasksProvider
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LoggingContext> _logger;

        public ScheduledTasksProvider(IMediator mediator, ILogger<LoggingContext> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ExecuteAllScheduledTasks()
        {
            var tasksToSchedule = TypeHelpers.GetImplementations<IScheduledTask>();
            var watch = new System.Diagnostics.Stopwatch();
            foreach (var type in tasksToSchedule)
            {
                try
                {
                    watch.Restart();
                    object? taskToSchedule = Activator.CreateInstance(type);
                    if (taskToSchedule == null)
                    {
                        _logger.LogWarning($"Cannot create instance of type task to schedule: {type.Name}, ignoring... ");
                        continue;
                    }
                    await _mediator.Publish(taskToSchedule);
                    watch.Stop();
                    _logger.LogInformation($"Task {type.Name} executed successfully. Duration={watch.ElapsedMilliseconds} ms");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during executing scheduled task");
                }
            }
        }
    }
}
