using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Irrigation;
using SmartHome.Domain.Entities.Devices.Greenhouse;

namespace SmartHome.Application.Events.Devices.GreenhouseController.Irrigation
{
    public class GreenhouseControllerIrrigationFinishedUpdateDbHandler : INotificationHandler<IrrigationFinishedEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GreenhouseControllerIrrigationFinishedUpdateDbHandler(IApplicationDbContext applicationDbContext) =>
            _applicationDbContext = applicationDbContext;

        public Task Handle(IrrigationFinishedEvent notification, CancellationToken cancellationToken)
        {
            AddGreenhouseIrrigationHistory(notification);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private void AddGreenhouseIrrigationHistory(IrrigationFinishedEvent notification)
        {
            _applicationDbContext.GreenhouseIrrigationHistory.Add(new GreenhouseIrrigationHistory
            {
                AverageWaterFlow = notification.AverageWaterFlow.ToFixed(),
                MaxWaterFlow = notification.MaxWaterFlow.ToFixed(),
                MinWaterFlow = notification.MinWaterFlow.ToFixed(),
                TotalWaterVolume = notification.TotalWaterVolume.ToFixed(),
                MeasurementEndTime = notification.IrrigationEndTime,
                MeasurementStartTime = notification.IrrigationStartTime
            });
        }
    }
}