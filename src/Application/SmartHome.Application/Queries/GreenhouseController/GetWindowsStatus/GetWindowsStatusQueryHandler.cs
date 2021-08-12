using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;

namespace SmartHome.Application.Queries.GreenhouseController.GetWindowsStatus
{
    public class GetWindowsStatusQueryHandler : IRequestHandler<GetWindowsStatusQuery, WindowsStatusVm>
    {
        private readonly IEventStoreClient _eventStoreClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetWindowsStatusQueryHandler(IEventStoreClient eventStoreClient, IDateTimeProvider dateTimeProvider)
        {
            _eventStoreClient = eventStoreClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<WindowsStatusVm> Handle(GetWindowsStatusQuery request, CancellationToken cancellationToken)
        {
            var currentDateTime = _dateTimeProvider.GetUtcNow();
            var windowsStatusTask = _eventStoreClient.FindEventsByCriteriaAsync(new GetEventsQuery
            {
                EventName = nameof(WindowsControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1,
                From = currentDateTime.AddDays(-1),
                EventType = EventType.General

            }, cancellationToken);

            var doorStatusTask = _eventStoreClient.FindEventsByCriteriaAsync(new GetEventsQuery
            {
                EventName = nameof(GreenhouseControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1,
                From = currentDateTime.AddDays(-1),
                EventType = EventType.General
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