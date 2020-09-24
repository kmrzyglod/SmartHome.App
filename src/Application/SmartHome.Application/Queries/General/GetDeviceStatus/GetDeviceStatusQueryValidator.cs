using FluentValidation;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Domain.Const;

namespace SmartHome.Application.Queries.General.GetDeviceStatus
{
    public class GetDeviceStatusQueryValidator:  AbstractValidator<GetDeviceStatusQuery>
    {
        public GetDeviceStatusQueryValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationMessages.PropertyNull);
        }
    }
}
