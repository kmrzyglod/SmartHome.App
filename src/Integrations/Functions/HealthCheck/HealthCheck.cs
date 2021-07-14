using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Functions.Worker;
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

        [Function("HealthCheck")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, FunctionContext context)
        {
            await _mediator.Publish(new HealthCheckEvent());
        }
    }
}