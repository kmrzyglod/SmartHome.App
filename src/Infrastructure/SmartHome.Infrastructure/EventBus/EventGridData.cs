namespace SmartHome.Infrastructure.EventBus
{
    public class EventGridData
    {
        public Properties properties { get; set; } = new Properties();
        public string body { get; set; } = string.Empty;

        public class Properties
        {
            public string MessageType { get; set; } = string.Empty;
        }
    }
}