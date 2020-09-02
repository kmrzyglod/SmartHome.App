using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DeviceCommandBus;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow;

namespace SmartHome.Application.Commands.Devices.WindowsController.OpenWindow
{
    public class OpenWindowCommandHandler: IRequestHandler<OpenWindowCommand>
    {
        private readonly IDeviceCommandBus _deviceCommandBus;

        public OpenWindowCommandHandler(IDeviceCommandBus deviceCommandBus)
        {
            _deviceCommandBus = deviceCommandBus;
        }

        public async Task<Unit> Handle(OpenWindowCommand request, CancellationToken cancellationToken)
        {
            await _deviceCommandBus.SendCommandAsync(request);
            return Unit.Value;
        }
    }
}
