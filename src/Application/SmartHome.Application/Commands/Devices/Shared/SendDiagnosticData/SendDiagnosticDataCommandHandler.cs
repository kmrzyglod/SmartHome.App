using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DeviceCommandBus;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;

namespace SmartHome.Application.Commands.Devices.Shared.SendDiagnosticData
{
    public class SendDiagnosticDataCommandHandler : IRequestHandler<SendDiagnosticDataCommand>
    {
        private readonly IDeviceCommandBus _deviceCommandBus;

        public SendDiagnosticDataCommandHandler(IDeviceCommandBus deviceCommandBus)
        {
            _deviceCommandBus = deviceCommandBus;
        }

        public async Task<Unit> Handle(SendDiagnosticDataCommand request, CancellationToken cancellationToken)
        {
            await _deviceCommandBus.SendCommandAsync(request);
            return Unit.Value;
        }
    }
}