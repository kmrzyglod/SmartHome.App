using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;

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