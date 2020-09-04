using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;

namespace SmartHome.Application.Commands.Devices.GreenhouseController.Irrigation
{
    public class IrrigateCommandValidator: AbstractValidator<IrrigateCommand>
    {
        public IrrigateCommandValidator()
        {
            Include(new CommandValidatorBase());
        }
    }
}
