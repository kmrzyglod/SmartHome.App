using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DeviceCommandBus;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;

namespace SmartHome.Application.Commands.Devices.WindowsController.CloseWindow
{
    public class CloseWindowCommandHandler: IRequestHandler<CloseWindowCommand>
    {
        private readonly IDeviceCommandBus _deviceCommandBus;

        public CloseWindowCommandHandler(IDeviceCommandBus deviceCommandBus)
        {
            _deviceCommandBus = deviceCommandBus;
        }

        public async Task<Unit> Handle(CloseWindowCommand request, CancellationToken cancellationToken)
        {
            await _deviceCommandBus.SendCommandAsync(request);
            return Unit.Value;
        }
    }
}
