namespace SmartHome.Infrastructure.EventBus
{
    public class EventGridData
    {
        public Properties properties { get; set; }
        public string body { get; set; }

        public class Properties
        {
            public string MessageType { get; set; }
        }
    }
}