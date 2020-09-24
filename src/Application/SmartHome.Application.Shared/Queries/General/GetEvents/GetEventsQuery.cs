using System;
using MediatR;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.General.GetEvents
{
    public class GetEventsQuery: PageRequest, IEventFilteringCriteria, IRequest<PaginationResult<EventVm>>
    {
        //Filters
        public DateTime? From { get;set; }
        public DateTime? To { get;set; }
        public EventType? EventType { get;set; }
        public string? EventName { get;set; } = string.Empty;
        public string? Source { get;set; } = string.Empty;
    }
}
