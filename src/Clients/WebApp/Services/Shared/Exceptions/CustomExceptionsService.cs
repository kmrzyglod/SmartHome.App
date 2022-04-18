using System;

namespace SmartHome.Clients.WebApp.Services.Shared.Exceptions;

public class CustomExceptionsService : ICustomExceptionsService
{
    public EventHandler<Exception>? OnExceptionThrown { get; set; }
    public EventHandler<ExceptionsCancelledEvent>? OnExceptionsCancelled { get; set; }

    public void ThrowException(Exception e)
    {
        OnExceptionThrown?.Invoke(this, e);
    }

    public void CancelExceptions()
    {
        OnExceptionsCancelled?.Invoke(this, new ExceptionsCancelledEvent());
    }

    public class ExceptionsCancelledEvent
    {
    }
}