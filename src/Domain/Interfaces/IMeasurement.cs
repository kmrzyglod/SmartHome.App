using System;

namespace SmartHome.Domain.Interfaces
{
    public interface IMeasurement
    {
        DateTime MeasurementStartTime { get; set; }
        DateTime MeasurementEndTime { get; set; }
    }
}