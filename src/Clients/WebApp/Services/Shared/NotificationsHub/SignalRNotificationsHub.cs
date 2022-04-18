using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Clients.WebApp.Services.Shared.ApiUrlProvider;
using SmartHome.Clients.WebApp.Services.Shared.AuthTokenProvider;
using SmartHome.Clients.WebApp.Services.Shared.Exceptions;

namespace SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

public class SignalRNotificationsHub : IDisposable, INotificationsHub
{
    private readonly HubConnection _hubConnection;

    private readonly ConcurrentDictionary<string, Action> _signalRSubscriptions =
        new();

    private readonly ConcurrentDictionary<SubscriptionKey, IEnumerable<Func<object, Task>>> _subscriptions =
        new();


    public SignalRNotificationsHub(IApiUrlProviderService apiUrlProvider, IAuthTokenProviderService tokenProvider,
        ICustomExceptionsService customExceptionsService)
    {
        var customExceptionsService1 = customExceptionsService;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(apiUrlProvider.GetSignalRUrl(),
                options => { options.AccessTokenProvider = tokenProvider.GetToken; })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.Closed += exception =>
        {
            Console.WriteLine($"Connection to hub closed: {exception}");
            if (exception != null)
            {
                customExceptionsService1.ThrowException(exception);
            }

            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += s =>
        {
            Console.WriteLine($"Reconnected to hub: {s}");
            customExceptionsService1.CancelExceptions();
            return Task.CompletedTask;
        };

        _hubConnection.Reconnecting += exception =>
        {
            Console.WriteLine($"Reconnecting to hub: {exception?.Message}");
            customExceptionsService1.ThrowException(new HubReconnectingException());
            return Task.CompletedTask;
        };
    }

    public void Dispose()
    {
        Console.WriteLine("disposing hub ");

        _ = _hubConnection.DisposeAsync();
    }

    public async Task ConnectAsync()
    {
        await _hubConnection
            .StartAsync();
        Console.WriteLine("Connected to SignalR hub");
    }

    public void Unsubscribe(string subscriptionId)
    {
        var keysToUnsubscribe =
            _subscriptions.Where(x => x.Key.SubscriptionId == subscriptionId)
                .Select(x => x.Key);

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
        Subscribe(typeof(TMessage), subscriptionId, args => handler((TMessage)args));
    }

    public void Subscribe<TMessage>(string subscriptionId, Func<Task> handler)
        where TMessage : class
    {
        Subscribe(typeof(TMessage), subscriptionId, args => handler());
    }

    public void Subscribe(Type eventType, string subscriptionId, Func<object, Task> handler)
    {
        _subscriptions.AddOrUpdate(new SubscriptionKey { SubscriptionId = subscriptionId, MethodName = eventType.Name },
            key =>
            {
                _signalRSubscriptions.AddOrUpdate(eventType.Name, s =>
                {
                    var result = () =>
                    {
                        _hubConnection.On(eventType.Name, new[] { typeof(object) }, args =>
                        {
                            var tasks = _subscriptions.Where(x => x.Key.MethodName == eventType.Name)
                                .SelectMany(x => x.Value)
                                .Select(task =>
                                {
                                    try
                                    {
                                        return task(JsonSerializerHelpers.DeserializeFromObject(args[0], eventType,
                                            CustomJsonSerializerOptionsProvider.OptionsWithCaseInsensitive));
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(
                                            $"[SignalRNotificationsHub] Error during event deserialization: {e.Message}");
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

            return Equals((SubscriptionKey)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MethodName, SubscriptionId);
        }
    }
}