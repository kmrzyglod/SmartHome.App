using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.WindowsController.WindowOpened
{
    public class WindowOpenedEvent: IEvent
    {
        public ushort WindowId { get; set; }
        public string Source { get; set; }  = string.Empty;
    }
}
