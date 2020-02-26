using System;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Message;

namespace SmartHome.Application.Interfaces.Event
{
    public interface ICommandResultEvent : IEvent, IMessage
    {
        StatusCode Status { get; }
        string ErrorMessage { get; }
    }
}