using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public class SignalRNotificationsHub: IDisposable, INotificationsHub
    {
        private const string SIGNALR_HUB_URL = "https://km-smart-home-api.azurewebsites.net/api/v1/NotificationsHub";
        private HubConnection _hubConnection;

        public SignalRNotificationsHub()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(SIGNALR_HUB_URL)
                .Build();
        }

        public void Subscribe<TMessage>(string methodName, Action<TMessage> handler)
        {
            _hubConnection.On(methodName, handler);
        }

        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            _hubConnection.On(typeof(TMessage).Name, handler);
        }

        public Task ConnectAsync()
        {
            return _hubConnection.StartAsync();
        }

        public void Dispose()
        {
            _ = _hubConnection.DisposeAsync();
        }
    }
}
