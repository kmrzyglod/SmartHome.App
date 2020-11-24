using System;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public interface INotificationsHub
    {
        void Subscribe<TMessage>(string methodName, Action<TMessage> handler);
        void Subscribe<TMessage>(Action<TMessage> handler);
        Task ConnectAsync();
        void Dispose();
    }
}