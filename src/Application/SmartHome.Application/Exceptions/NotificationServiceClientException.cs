using System;

namespace SmartHome.Application.Exceptions
{
    public class NotificationServiceClientException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}