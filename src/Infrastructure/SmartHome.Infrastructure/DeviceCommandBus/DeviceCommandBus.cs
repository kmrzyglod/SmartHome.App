using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using SmartHome.Application.Interfaces.Command;
using SmartHome.Application.Interfaces.DeviceCommandBus;

namespace SmartHome.Infrastructure.DeviceCommandBus
{
    public class DeviceCommandBus: IDeviceCommandBus
    {
        private readonly ServiceClient _iotHubClient;

        public DeviceCommandBus(ServiceClient iotHubClient)
        {
            _iotHubClient = iotHubClient;
        }

        public Task SendCommandAsync(IDeviceCommand command)
        {
            var commandMessage = new
                Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(command)));

            commandMessage.Properties.Add(new KeyValuePair<string, string>("command-name", command.GetType().Name));

            return _iotHubClient.SendAsync(command.TargetDeviceId, commandMessage);
        }
    }
}
