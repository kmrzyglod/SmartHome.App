using System;

namespace SmartHome.Clients.WebApp.Services.Shared.Exceptions;

public interface ICustomExceptionsService
{
    EventHandler<Exception>? OnExceptionThrown { get; set; }
    EventHandler<CustomExceptionsService.ExceptionsCancelledEvent>? OnExceptionsCancelled { get; set; }
    void ThrowException(Exception e);
    void CancelExceptions();
}