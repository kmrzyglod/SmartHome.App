namespace SmartHome.Application.Shared.Interfaces.Event
{
    public interface IPageRequest
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}
