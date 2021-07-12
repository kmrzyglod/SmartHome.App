﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub
{
    public class SignalRNotificationsHub : IDisposable, INotificationsHub
    {
        private const string SIGNALR_HUB_URL = "https://km-smart-home-api.azurewebsites.net/api/v1/NotificationsHub";
        private readonly HubConnection _hubConnection;

        private readonly ConcurrentDictionary<SubscriptionKey, IEnumerable<Action<object>>> _subscriptions =
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


        public void Subscribe<TMessage>(string subscriptionId, Action<TMessage> handler)
            where TMessage : class
        {
            var methodName = typeof(TMessage).Name;

            _subscriptions.AddOrUpdate(new SubscriptionKey {SubscriptionId = subscriptionId, MethodName = methodName},
                key =>
                {
                    _signalRSubscriptions.AddOrUpdate(methodName, s =>
                    {
                        Action result = () =>
                        {
                            _hubConnection.On<TMessage>(methodName, (TMessage arg) =>
                            {
                                foreach (var action in _subscriptions.Where(x => x.Key.MethodName == methodName)
                                    .SelectMany(x => x.Value))
                                {
                                    action(arg);
                                }
                            });
                        };
                        result();
                        return result;
                    }, (s, action) => action);

                    return new List<Action<object>>
                    {
                        arg => { handler((TMessage) arg); }
                    };
                }, (key, actions) => { return actions.Append(arg => { handler((TMessage) arg); }); });
        }

        
        public void Subscribe(string methodName, string subscriptionId, Action<object> handler)
        {

            _subscriptions.AddOrUpdate(new SubscriptionKey {SubscriptionId = subscriptionId, MethodName = methodName},
                key =>
                {
                    _signalRSubscriptions.AddOrUpdate(methodName, s =>
                    {
                        Action result = () =>
                        {
                            _hubConnection.On(methodName, new[] {typeof(object)}, (args) =>
                            {
                                foreach (var action in _subscriptions.Where(x => x.Key.MethodName == methodName)
                                    .SelectMany(x => x.Value))
                                {
                                    action(args[0]);
                                }

                                return Task.CompletedTask;
                            });
                        };
                        result();
                        return result;
                    }, (s, action) => action);

                    return new List<Action<object>>
                    {
                        arg => { handler( arg); }
                    };
                }, (_, actions) => actions.Append(handler));
        }

        private class SubscriptionKey
        {
            public string MethodName { get; set; }
            public string SubscriptionId { get; set; }

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