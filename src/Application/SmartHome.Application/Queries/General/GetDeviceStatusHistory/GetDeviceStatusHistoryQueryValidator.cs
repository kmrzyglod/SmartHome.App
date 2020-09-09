using FluentValidation;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatusHistory;
using SmartHome.Domain.Const;

namespace SmartHome.Application.Queries.General.GetDeviceStatusHistory
{
    public class GetDeviceStatusHistoryQueryValidator:  AbstractValidator<GetDeviceStatusHistoryQuery>
    {
        public GetDeviceStatusHistoryQueryValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationMessages.PropertyNull);
        }
    }
}
