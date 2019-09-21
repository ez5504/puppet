namespace Puppet.Common.StateManagement 
{
    using System;

    public interface IWeatherData
    {
        string CurrentCondition { get; set; }
        bool? IsDaytime { get; set; }
        DateTime? SunriseTime { get; set; }
        DateTime? SunsetTime { get; set; }
        decimal? TemperatureFeelsLike { get; set; }
        decimal? TemperatureActual { get; set; }
        decimal? Humidity { get; set; }
        DateTime LastUpdated { get; set; }
    }
}
