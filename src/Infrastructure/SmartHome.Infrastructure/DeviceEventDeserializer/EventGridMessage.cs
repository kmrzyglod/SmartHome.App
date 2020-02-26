namespace SmartHome.Infrastructure.DeviceEventDeserializer
{
    public class EventGridMessage
    {
        public Props Properties { get; set; }
        public string Body { get; set; }

        public class Props
        {
            public string MessageType { get; set; }
        }
    }
}