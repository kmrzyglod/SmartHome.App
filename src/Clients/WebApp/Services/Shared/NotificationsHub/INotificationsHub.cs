using System;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public interface INotificationsHub
    {
        void Subscribe<TMessage>(string subscriptionId, Func<TMessage, Task> handler)
            where TMessage : class;

        void Subscribe(string methodName, string subscriptionId, Func<object, Task> handler);
        Task ConnectAsync();
        void Dispose();
        void Unsubscribe(string subscriptionId);
    }
}