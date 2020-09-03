using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.WindowsController.WindowClosed
{
    public class WindowClosedEvent: IEvent
    {
        public ushort WindowId { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
