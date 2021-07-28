using System;
using System.Collections.Generic;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;

namespace SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor
{
    public static class DefaultCommandNotificationMessagesMapping
    {
        private static IDictionary<string, DefaultCommandNotificationMessage> Messages { get; set; } =
            new Dictionary<string, DefaultCommandNotificationMessage>
            {
                {
                    nameof(PingCommand), new DefaultCommandNotificationMessage
                    {
                        SuccessMessage = "Ping response received",
                        ErrorMessage = "Error during sending ping to device. Description {0}",
                        TimeoutMessage = "Ping timeout",
                        ProcessingStartMessage = "Ping request was sent to device"
                    }
                },
                {
                    nameof(SendDiagnosticDataCommand), new DefaultCommandNotificationMessage
                    {
                        SuccessMessage = "Diagnostic data refreshed",
                        ErrorMessage = "Error during refreshing diagnostic data. Description {0}",
                        TimeoutMessage = "Timeout during refreshing diagnostic data",
                        ProcessingStartMessage = "Refresh diagnostic data request was send to device"
                    }
                }
            };

        public static DefaultCommandNotificationMessage GetDefaultMessage(string commandName, Guid correlationId)
        {
            if (!Messages.TryGetValue(commandName, out var defaultCommandNotificationMessage))
            {
                defaultCommandNotificationMessage = new DefaultCommandNotificationMessage
                {
                    ErrorMessage =
                        $"Error during processing command {commandName} with correlation id: {correlationId}. Error: {0}",
                    ProcessingStartMessage = $"Command {commandName} with correlation id {correlationId} was sent",
                    SuccessMessage =
                        $"Command {commandName} with correlation id {correlationId} processed successfully",
                    TimeoutMessage =
                        $"Timeout during processing command {commandName} with correlation id {correlationId} "
                };
            }

            return defaultCommandNotificationMessage;
        }

        public class DefaultCommandNotificationMessage
        {
            public string SuccessMessage { get; init; } = string.Empty;
            public string ProcessingStartMessage { get; init; } = string.Empty;
            public string ErrorMessage { get; init; } = string.Empty;
            public string TimeoutMessage { get; init; } = string.Empty;
        }
    }
}
