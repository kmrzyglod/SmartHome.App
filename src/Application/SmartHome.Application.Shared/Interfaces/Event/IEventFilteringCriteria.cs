using SmartHome.Application.Shared.Enums;

namespace SmartHome.Application.Shared.Interfaces.Event
{
    public interface IEventFilteringCriteria
    {
        System.DateTime? From { get; set; }
        System.DateTime? To { get; set; }
        EventType? EventType { get; set; }
        string? EventName { get; set; }
        string? Source { get; set; }
    }
}