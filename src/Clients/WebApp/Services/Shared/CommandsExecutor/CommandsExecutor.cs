using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Radzen;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Models;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor
{
    public class CommandsExecutor : ICommandsExecutor, IDisposable
    {
        private readonly string _notificationHubSubscriptionId = Guid.NewGuid().ToString();

        public CommandsExecutor(INotificationsHub notificationsHub, NotificationService toastrNotificationService)
        {
            _notificationsHub = notificationsHub;
            _toastrNotificationService = toastrNotificationService;
            _notificationsHub.Subscribe<CommandResultEvent>(_notificationHubSubscriptionId, commandResult =>
            {
                if (!PendingCommands.TryRemove(commandResult.CorrelationId, out var pendingCommand))
                {
                    return Task.CompletedTask;
                }

                switch (commandResult.Status)
                {
                    case StatusCode.Success:
                        _toastrNotificationService.Notify(NotificationSeverity.Success, string.Empty,
                            pendingCommand.SuccessMessage);
                        break;
                    case StatusCode.Error:
                        _toastrNotificationService.Notify(NotificationSeverity.Error, string.Empty,
                            string.Format(pendingCommand.ErrorMessage, commandResult.ErrorMessage));
                        break;
                    case StatusCode.Refused:
                        _toastrNotificationService.Notify(NotificationSeverity.Error, string.Empty,
                            string.Format(pendingCommand.ErrorMessage, commandResult.ErrorMessage));
                        break;
                    case StatusCode.ValidationError:
                        _toastrNotificationService.Notify(NotificationSeverity.Error, string.Empty,
                            string.Format(pendingCommand.ErrorMessage, commandResult.ErrorMessage));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                pendingCommand.TimeOutTaskCancellation.Cancel();

                return Task.CompletedTask;
            });
        }

        private INotificationsHub _notificationsHub { get; } = null!;
        private NotificationService _toastrNotificationService { get; } = null!;

        private ConcurrentDictionary<Guid, CommandExecutingContext> PendingCommands { get; } = new();

        public async Task<CommandCorrelationId> ExecuteCommand<T>(Func<T, Task<CommandCorrelationId>> fnc,
            T command, int timeoutInSeconds = 60, string? processingStartMessage = default,
            string? successMessage = default,
            string? errorMessage = default, string? timeoutMessage = default) where T : ICommand
        {
            CommandCorrelationId result;
            string? commandName = command.GetType().Name;
            var defaultCommandNotificationMessages =
                DefaultCommandNotificationMessagesMapping.GetDefaultMessage(commandName, command.CorrelationId);
            timeoutMessage = string.IsNullOrEmpty(timeoutMessage)
                ? defaultCommandNotificationMessages.TimeoutMessage
                : timeoutMessage;

            errorMessage = string.IsNullOrEmpty(errorMessage)
                ? defaultCommandNotificationMessages.ErrorMessage
                : errorMessage;

            processingStartMessage = string.IsNullOrEmpty(processingStartMessage)
                ? defaultCommandNotificationMessages.ProcessingStartMessage
                : processingStartMessage;

            try
            {
                result = await fnc(command);
            }
            catch (Exception e)
            {
                _toastrNotificationService.Notify(NotificationSeverity.Error, string.Empty,
                    string.Format(errorMessage, e.Message));
                return new CommandCorrelationId(Guid.Empty);
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var commandExecutingContext = new CommandExecutingContext
            {
                Command = command,
                Timeout = timeoutInSeconds,
                SuccessMessage = string.IsNullOrEmpty(successMessage)
                    ? defaultCommandNotificationMessages.SuccessMessage
                    : successMessage,
                ErrorMessage = errorMessage,
                TimeOutMessage = timeoutMessage,
                TimeOutTask = new Task(async () =>
                {
                    try
                    {
                        await Task.Delay(timeoutInSeconds * 1000, cancellationTokenSource.Token);
                        if (!PendingCommands.TryRemove(result.CorrelationId, out var pendingCommand))
                        {
                            return;
                        }
                        _toastrNotificationService.Notify(NotificationSeverity.Warning, string.Empty, pendingCommand.TimeOutMessage);
                    }
                    catch (TaskCanceledException)
                    {
                        //nil
                    }
                }, cancellationTokenSource.Token),
                TimeOutTaskCancellation = cancellationTokenSource
            };

            commandExecutingContext.TimeOutTask.Start();
            PendingCommands.TryAdd(result.CorrelationId, commandExecutingContext);

            _toastrNotificationService.Notify(NotificationSeverity.Info, string.Empty,
                processingStartMessage);

            return result;
        }

        public void Dispose()
        {
            _notificationsHub.Unsubscribe(_notificationHubSubscriptionId);
        }

        private class CommandExecutingContext
        {
            public ICommand? Command { get; init; }
            public int Timeout { get; init; } = 60;
            public string SuccessMessage { get; init; } = string.Empty;
            public string ErrorMessage { get; init; } = string.Empty;
            public string TimeOutMessage { get; init; } = string.Empty;
            public Task? TimeOutTask { get; init; }
            public CancellationTokenSource TimeOutTaskCancellation { get; init; } = new();
        }
    }
}