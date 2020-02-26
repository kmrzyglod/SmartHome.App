using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Infrastructure.NotificationService
{
    public class NotificationServiceClient
    {
        private readonly HubConnection _signalrConnection;

        public NotificationServiceClient(HubConnection signalrConnection)
        {
            _signalrConnection = signalrConnection;
        }

        //public Task SendNotificationAsync(IEvent @event)
        //{
        //    //_signalrConnection.SendAsync()
        //}
    }
}