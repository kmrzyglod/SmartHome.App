using System;

namespace SmartHome.Application.Helpers
{
    public static class NumberHelpers
    {
        public static double ToFixed(this double number, ushort precision = 2)
        {
            return Math.Round(number, precision, MidpointRounding.ToEven);
        }
    }
}