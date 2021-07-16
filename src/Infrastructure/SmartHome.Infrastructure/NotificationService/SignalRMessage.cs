namespace SmartHome.Infrastructure.NotificationService
{
    public class SignalRMessage
    {
        public string Target { get; set; } = string.Empty;
        public object[] Arguments { get; set; } = new object[0];
    }
}