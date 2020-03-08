using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.GetEvents;

namespace SmartHome.Application.Queries.GetEvents
{
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PaginationResult<EventVm>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventStoreClient _eventStoreClient;

        public GetEventsQueryHandler(IEventStoreClient eventStoreClient, IDateTimeProvider dateTimeProvider)
        {
            _eventStoreClient = eventStoreClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PaginationResult<EventVm>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventStoreClient.FindEventsByCriteriaAsync(request, request.PageNumber,
                request.PageSize, cancellationToken);
            return new PaginationResult<EventVm>
            {
                ResultTotalCount = result.ResultTotalCount,
                PageSize = result.PageSize,
                PageNumber = result.PageNumber,
                Result = result.Result.Select(x => new EventVm
                {
                    Source = x.Source,
                    EventName = x.EventName,
                    EventData = x.EventData,
                    EventType = x.EventType,
                    Timestamp = x.Timestamp,
                    Id = x.Id.ToString()
                }).ToList()
            };
        }
    }
}