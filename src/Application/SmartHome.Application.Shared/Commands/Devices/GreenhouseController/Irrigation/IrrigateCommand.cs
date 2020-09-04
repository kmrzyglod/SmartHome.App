using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation
{
    public class IrrigateCommand: IRequest, IDeviceCommand
    {
        public int MaximumIrrigationTime { get; set; }
        //Water volume in liters 
        public int WaterVolume { get; set; }
        
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = null!;
    }
}
