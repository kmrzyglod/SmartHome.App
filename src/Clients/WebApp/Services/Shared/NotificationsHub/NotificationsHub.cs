using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public class NotificationsHub
    {
        private const string SIGNALR_HUB_URL = "https://km-smart-home-signalr.service.signalr.net/notifications";
        private HubConnection _hubConnection;

        public NotificationsHub()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(SIGNALR_HUB_URL)
                .Build();
            _hubConnection.StartAsync();
        }
    }
}
