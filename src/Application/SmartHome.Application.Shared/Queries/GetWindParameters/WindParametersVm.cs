using System;
using SmartHome.Domain.Enums;

namespace SmartHome.Application.Shared.Queries.GetWindParameters
{
    public class WindParametersVm
    {
        public WindDirection MostFrequentWindDirection { get; set; }
        public double MinWindSpeed { get; set; }
        public double MaxWindSpeed { get; set; }
        public double AverageWindSpeed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
