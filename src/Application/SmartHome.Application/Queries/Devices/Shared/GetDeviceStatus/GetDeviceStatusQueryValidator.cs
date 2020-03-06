using FluentValidation;
using SmartHome.Domain.Const;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatus
{
    public class GetDeviceStatusQueryValidator:  AbstractValidator<GetDeviceStatusQuery>
    {
        public GetDeviceStatusQueryValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationMessages.PropertyNull);
        }
    }
}
