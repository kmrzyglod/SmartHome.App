using FluentValidation;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Domain.Const;

namespace SmartHome.Application.BaseValidators
{
    internal class CommandValidatorBase : AbstractValidator<ICommand>
    {
        public CommandValidatorBase()
        {
            RuleFor(x => x.CorrelationId)
                .NotEmpty()
                .WithMessage(ValidationMessages.PropertyNull);
        }
    }
}