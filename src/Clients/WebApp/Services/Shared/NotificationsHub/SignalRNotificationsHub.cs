using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public class SignalRNotificationsHub : IDisposable, INotificationsHub
    {
        private const string SIGNALR_HUB_URL = "https://km-smart-home-api.azurewebsites.net/api/v1/NotificationsHub";
        private readonly HubConnection _hubConnection;

        private readonly ConcurrentDictionary<SubscriptionKey, IEnumerable<Func<object, Task>>> _subscriptions =
            new();

        private readonly ConcurrentDictionary<string, Action> _signalRSubscriptions =
            new();


        public SignalRNotificationsHub()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(SIGNALR_HUB_URL)
                .Build();
        }

        public void Dispose()
        {
            _ = _hubConnection.DisposeAsync();
        }

        public Task ConnectAsync()
        {
            return _hubConnection.StartAsync();
        }

        public void Unsubscribe(string subscriptionId)
        {
            var keysToUnsubscribe =
                _subscriptions.Where(x => x.Key.SubscriptionId == subscriptionId).Select(x => x.Key);

            foreach (var subscriptionKey in keysToUnsubscribe)
            {
                _subscriptions.Remove(subscriptionKey, out _);
                if (_subscriptions.Any(x => x.Key.MethodName == subscriptionKey.MethodName))
                {
                    continue;
                }

                _signalRSubscriptions.Remove(subscriptionKey.MethodName, out _);
                _hubConnection.Remove(subscriptionKey.MethodName);
            }
        }


        public void Subscribe<TMessage>(string subscriptionId, Func<TMessage, Task> handler)
            where TMessage : class
        {
            Subscribe(typeof(TMessage), subscriptionId, (args) => handler((TMessage)args));
        }

        public void Subscribe<TMessage>(string subscriptionId, Func<Task> handler)
            where TMessage : class
        {
            Subscribe(typeof(TMessage), subscriptionId, (args) => handler());
        }

        public void Subscribe(Type eventType, string subscriptionId, Func<object, Task> handler)
        {

            _subscriptions.AddOrUpdate(new SubscriptionKey {SubscriptionId = subscriptionId, MethodName = eventType.Name},
                key =>
                {
                    _signalRSubscriptions.AddOrUpdate(eventType.Name, s =>
                    {
                        Action result = () =>
                        {
                            _hubConnection.On(eventType.Name, new[] {typeof(object)},  (args) =>
                            {
                                var tasks = _subscriptions.Where(x => x.Key.MethodName == eventType.Name)
                                    .SelectMany(x => x.Value).Select(task =>
                                    {
                                        try
                                        {
                                            var deserialized = JsonSerializer.Deserialize(
                                                ((JsonElement) args[0]).GetRawText(),
                                                eventType)!;
                                            return task(deserialized);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine($"[SignalRNotificationsHub] Error during event deserialization: {e.Message}");
                                            return task(args[0]);
                                        }
                                    });

                                return Task.WhenAll(tasks);
                            });
                        };
                        result();
                        return result;
                    }, (s, action) => action);

                    return new List<Func<object, Task>>
                    {
                        handler
                    };
                }, (_, actions) => actions.Append(handler));
        }

        private class SubscriptionKey
        {
            public string MethodName { get; init; } = string.Empty;
            public string SubscriptionId { get; init; } = string.Empty;

            private bool Equals(SubscriptionKey other)
            {
                return MethodName == other.MethodName && SubscriptionId == other.SubscriptionId;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((SubscriptionKey) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MethodName, SubscriptionId);
            }
        }
    }
}