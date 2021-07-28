using System;
using Microsoft.AspNetCore.Components;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Shared.Components
{
    public abstract class ComponentWithNotificationHub : ComponentBase, IDisposable
    {
        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;

        protected string NotificationHubSubscriptionId { get; } = Guid.NewGuid()
            .ToString();

        public virtual void Dispose()
        {
            NotificationsHub.Unsubscribe(NotificationHubSubscriptionId);
        }
    }
}