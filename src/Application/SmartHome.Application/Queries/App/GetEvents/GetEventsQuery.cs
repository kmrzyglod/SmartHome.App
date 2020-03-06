using System;
using MediatR;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.App.GetEvents
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
