using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using SmartHome.Integrations.Functions.ScheduleTasks.Infrastructure.ScheduledTasksProvider;

namespace SmartHome.Integrations.Functions.ScheduleTasks
{
    public class ScheduleTasks
    {
        private readonly IScheduledTasksProvider _scheduledTasksProvider;

        public ScheduleTasks(IScheduledTasksProvider scheduledTasksProvider)
        {
            _scheduledTasksProvider = scheduledTasksProvider;
        }

        [Function("ScheduleTasks")]
        public Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer, FunctionContext context)
        {
            return _scheduledTasksProvider.ExecuteAllScheduledTasks();
        }
    }
}