namespace SmartHome.Application.Shared.Events.Devices.WindowsController.WindowOpened
{
    public class WindowOpenedEvent: EventBase
    {
        public ushort WindowId { get; set; }
    }
}
