using FluentValidation;
using SmartHome.Application.BaseValidators;

namespace SmartHome.Application.Commands.Devices.Shared.Ping
{
    internal class PingCommandValidator : AbstractValidator<PingCommand>
    {
        public PingCommandValidator()
        {
            Include(new CommandValidatorBase());
        }
    }
}