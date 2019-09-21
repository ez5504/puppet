namespace Puppet.Common.StateManagement 
{
    using System;

    public class WeatherData : IWeatherData
    {
        public string CurrentCondition { get; set; }
        public bool? IsDaytime { get; set; }
        public DateTime? SunriseTime { get; set; }
        public DateTime? SunsetTime { get; set; }
        public decimal? TemperatureFeelsLike { get; set; }
        public decimal? TemperatureActual { get; set; }
        public decimal? Humidity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now.AddYears(-5);
    }
}
