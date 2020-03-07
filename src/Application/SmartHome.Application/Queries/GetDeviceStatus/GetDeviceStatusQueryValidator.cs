using FluentValidation;
using SmartHome.Application.Shared.Queries.GetDeviceStatus;
using SmartHome.Domain.Const;

namespace SmartHome.Application.Queries.GetDeviceStatus
{
    public class GetDeviceStatusQueryValidator:  AbstractValidator<GetDeviceStatusQuery>
    {
        public GetDeviceStatusQueryValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationMessages.PropertyNull);
        }
    }
}
