namespace SmartHome.Application.Shared.Events.Devices.WindowsController.WindowClosed
{
    public class WindowClosedEvent: EventBase
    {
        public ushort WindowId { get; set; }
    }
}
