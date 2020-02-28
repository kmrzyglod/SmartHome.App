using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DeviceCommandBus;

namespace SmartHome.Application.Commands.Devices.Shared.Ping
{
    internal class PingCommandHandler : IRequestHandler<PingCommand>
    {
        private readonly IDeviceCommandBus _deviceCommandBus;

        public PingCommandHandler(IDeviceCommandBus deviceCommandBus)
        {
            _deviceCommandBus = deviceCommandBus;
        }

        public async Task<Unit> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            await _deviceCommandBus.SendCommandAsync(request);
            return Unit.Value;
        }
    }
}