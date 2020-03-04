using FluentValidation;
using SmartHome.Application.BaseValidators;

namespace SmartHome.Application.Commands.Devices.Shared.SendDiagnosticData
{
    public class SendDiagnosticDataCommandValidator : AbstractValidator<SendDiagnosticDataCommand>
    {
        public SendDiagnosticDataCommandValidator()
        {
            Include(new CommandValidatorBase());
        }
    }
}