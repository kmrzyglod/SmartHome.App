using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SmartHome.Application.Interfaces.EventBus;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Infrastructure.MediatR
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventBus _eventBus;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IEventBus eventBus,
            IDateTimeProvider dateTimeProvider)
        {
            _validators = validators;
            _eventBus = eventBus;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any() && request is ICommand command)
            {
                await _eventBus.Publish(CommandResultEvent.CreateAppCommandResult(command, StatusCode.ValidationError,
                    _dateTimeProvider.GetUtcNow(), JsonSerializer.Serialize(failures)));
            }

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}