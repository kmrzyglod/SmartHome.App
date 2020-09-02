using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;

namespace SmartHome.Application.Commands.Devices.WindowsController.CloseWindow
{
    public class CloseWindowCommandValidator: AbstractValidator<CloseWindowCommand>
    {
        public CloseWindowCommandValidator()
        {
            Include(new CommandValidatorBase());
        }
    }
}
