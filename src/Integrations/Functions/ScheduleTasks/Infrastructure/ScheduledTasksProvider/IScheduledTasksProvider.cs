using System.Threading.Tasks;

namespace SmartHome.Integrations.Functions.ScheduleTasks.Infrastructure.ScheduledTasksProvider
{
    public interface IScheduledTasksProvider
    {
        Task ExecuteAllScheduledTasks();
    }
}