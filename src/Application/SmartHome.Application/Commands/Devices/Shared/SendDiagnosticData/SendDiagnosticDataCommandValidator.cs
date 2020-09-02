using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;

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