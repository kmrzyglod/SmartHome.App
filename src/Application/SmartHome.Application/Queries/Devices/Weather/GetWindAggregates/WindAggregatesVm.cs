using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Application.Queries.Devices.Weather.GetWindAggregates
{
    public class WindAggregatesVm
    {
        public double AverageWindSpeed { get; set; }
        public double MaxWindSpeed { get; set; }
        public double MinWindSpeed { get; set; }
        public DateTime MinWindSpeedTimestamp{ get; set; }
        public DateTime MaxWindSpeedTimestamp{ get; set; }
    }
}
