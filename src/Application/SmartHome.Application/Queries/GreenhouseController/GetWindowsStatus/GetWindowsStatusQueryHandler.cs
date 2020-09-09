using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Resources;
using MediatR;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;

namespace SmartHome.Application.Queries.GreenhouseController.GetWindowsStatus
{
    public class GetWindowsStatusQueryHandler: IRequestHandler<GetWindowsStatusQuery, WindowsStatusVm>
    {
        private readonly IEventStoreClient _eventStoreClient;

        public GetWindowsStatusQueryHandler(IEventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public Task<WindowsStatusVm> Handle(GetWindowsStatusQuery request, CancellationToken cancellationToken)
        {
            //var lastWindow1_eventStoreClient.FindEventsByCriteriaAsync()
            return Task.FromResult(new WindowsStatusVm());
        }
    }
}
