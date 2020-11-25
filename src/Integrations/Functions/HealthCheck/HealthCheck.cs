using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Shared.Events.App;

namespace SmartHome.Integrations.Functions.HealthCheck
{
    public class HealthCheck
    {
        private readonly IMediator _mediator;

        public HealthCheck(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("HealthCheck")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            await _mediator.Publish(new HealthCheckEvent());
        }
    }
}