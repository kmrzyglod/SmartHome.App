using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;

namespace SmartHome.Application.Queries.GreenhouseController.GetWindowsStatus
{
    public class GetWindowsStatusQueryHandler : IRequestHandler<GetWindowsStatusQuery, WindowsStatusVm>
    {
        private readonly IEventStoreClient _eventStoreClient;

        public GetWindowsStatusQueryHandler(IEventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public async Task<WindowsStatusVm> Handle(GetWindowsStatusQuery request, CancellationToken cancellationToken)
        {
            var windowsStatusTask = _eventStoreClient.FindEventsByCriteriaAsync(new GetEventsQuery
            {
                EventName = nameof(WindowsControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1
            }, cancellationToken);

            var doorStatusTask = _eventStoreClient.FindEventsByCriteriaAsync(new GetEventsQuery
            {
                EventName = nameof(GreenhouseControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1
            }, cancellationToken);

            var events = await Task.WhenAll(windowsStatusTask, doorStatusTask);
            var windowsStatus =
                (events.First(x => x.Result.First().EventName == nameof(WindowsControllerTelemetryEvent)).Result.First()
                    .EventData as WindowsControllerTelemetryEvent)!;

            var doorStatus = (events
                .First(x => x.Result.First().EventName == nameof(GreenhouseControllerTelemetryEvent)).Result.First()
                .EventData as GreenhouseControllerTelemetryEvent)!;

            return new WindowsStatusVm
            {
                Door = doorStatus.IsDoorOpen,
                DoorLastStatusUpdate = doorStatus.Timestamp,
                Window1 = windowsStatus.WindowsStatus[0],
                Window2 = windowsStatus.WindowsStatus[1],
                WindowsLastStatusUpdate = windowsStatus.Timestamp
            };
        }
    }
}