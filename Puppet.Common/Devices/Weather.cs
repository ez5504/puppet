
using Puppet.Common.Services;
using System.Threading.Tasks;
using Puppet.Common.StateManagement;
using System;

namespace Puppet.Common.Devices
{
    public class Weather : DeviceBase
    {
        public IWeatherData Data;

        public Weather(IWeatherData data, HomeAutomationPlatform hub, string id) : base(hub, id)
        {
            Data = data;
        }

        public async Task<bool> IsDaytime()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.IsDaytime.HasValue)
            {
                await Update();
            }
            return Data.IsDaytime.Value;
        }

        public async Task<string> CurrentCondition()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !string.IsNullOrWhiteSpace(Data.CurrentCondition))
            {
                await Update();
            }
            return Data.CurrentCondition;
        }

        public async Task<DateTime> SunriseTime()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.SunriseTime.HasValue)
            {
                await Update();
            }
            return Data.SunriseTime.Value;
        }

        public async Task<DateTime> SunsetTime()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.SunsetTime.HasValue)
            {
                await Update();
            }
            return Data.SunsetTime.Value;
        }

        public async Task<decimal> TemperatureFeelsLike()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.TemperatureFeelsLike.HasValue)
            {
                await Update();
            }
            return Data.TemperatureFeelsLike.Value;
        }

        public async Task<decimal> TemperatureActual()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.TemperatureActual.HasValue)
            {
                await Update();
            }
            return Data.TemperatureActual.Value;
        }

        public async Task<decimal> Humidity()
        {
            if (((DateTime.Now - Data.LastUpdated).TotalMinutes > 60) || !Data.Humidity.HasValue)
            {
                await Update();
            }
            return Data.Humidity.Value;
        }

        public async Task Update()
        {
            await this.DoAction("pollData");
            UpdateCachedData();
        }

        private void UpdateCachedData()
        {
            Data.CurrentCondition = GetState()["condition_text"];
            Data.IsDaytime = bool.Parse(GetState()["is_day"]);
            Data.SunriseTime = DateTime.Parse(GetState()["localSunrise"]);
            Data.SunsetTime = DateTime.Parse(GetState()["localSunset"]);
            Data.TemperatureFeelsLike = decimal.Parse(GetState()["feelsLike"]);
            Data.TemperatureActual = decimal.Parse(GetState()["temperature"]);
            Data.Humidity = decimal.Parse(GetState()["humidity"]);
            Data.LastUpdated = DateTime.Parse(GetState()["last_poll_Forecast"]);
        }
    }
}
