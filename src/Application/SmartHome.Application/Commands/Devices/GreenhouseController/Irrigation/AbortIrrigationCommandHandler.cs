using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DeviceCommandBus;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;

namespace SmartHome.Application.Commands.Devices.GreenhouseController.Irrigation
{
    public class AbortIrrigationCommandHandler: IRequestHandler<AbortIrrigationCommand>
    {
        private readonly IDeviceCommandBus _deviceCommandBus;

        public AbortIrrigationCommandHandler(IDeviceCommandBus deviceCommandBus)
        {
            _deviceCommandBus = deviceCommandBus;
        }

        public async Task<Unit> Handle(AbortIrrigationCommand request, CancellationToken cancellationToken)
        {
            await _deviceCommandBus.SendCommandAsync(request);
            return Unit.Value;
        }
    }
}
