using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow;

namespace SmartHome.Application.Commands.Devices.WindowsController.OpenWindow
{
    public class OpenWindowCommandValidator: AbstractValidator<OpenWindowCommand>
    {
        public OpenWindowCommandValidator()
        {
            Include(new CommandValidatorBase());
        }
    }
}
