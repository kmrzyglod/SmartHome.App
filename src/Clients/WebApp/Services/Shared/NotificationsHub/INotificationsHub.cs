using System;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public interface INotificationsHub
    {
        void Subscribe<TMessage>(string subscriptionId, Func<TMessage, Task> handler)
            where TMessage : class;

        void Subscribe(Type eventType, string subscriptionId, Func<object, Task> handler);

        void Subscribe<TMessage>(string subscriptionId, Func<Task> handler)
            where TMessage : class;
        Task ConnectAsync();
        void Dispose();
        void Unsubscribe(string subscriptionId);
    }
}