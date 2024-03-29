﻿using System;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor
{
    public interface ICommandsExecutor
    {
        public Task<CommandCorrelationId> ExecuteCommand<T>(Func<T, Task<CommandCorrelationId>> fnc, T command,
            int timeoutInSeconds = 60, Action<CommandResultEvent>? onCommandExecuted = null, string? processingStartMessage = default, string? successMessage = default,
            string? errorMessage = default,
            string? timeoutMessage = default, bool hideNotifications = false) where T : ICommand;
    }
}