using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.EventBus;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Infrastructure.MediatR
{
    public class CommandResultBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand, IRequest<TResponse> where TResponse : ICommandResultEvent
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventBus _eventBus;

        public CommandResultBehaviour(IEventBus eventBus, IDateTimeProvider dateTimeProvider)
        {
            _eventBus = eventBus;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var result = await next();
                await _eventBus.Publish(result);
                return result;
            }
            catch (Exception e)
            {
                await _eventBus.Publish(CommandResultEvent.CreateAppCommandResult(request, StatusCode.Error,
                    _dateTimeProvider.GetUtcNow(), e.Message));
                throw;
            }
        }
    }
}