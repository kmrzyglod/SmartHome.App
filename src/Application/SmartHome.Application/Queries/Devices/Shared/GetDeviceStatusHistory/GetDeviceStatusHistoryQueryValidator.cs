using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SmartHome.Domain.Const;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatusHistory
{
    public class GetDeviceStatusHistoryQueryValidator:  AbstractValidator<GetDeviceStatusHistoryQuery>
    {
        public GetDeviceStatusHistoryQueryValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationMessages.PropertyNull);
        }
    }
}
