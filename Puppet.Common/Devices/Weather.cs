
using Puppet.Common.Services;
using System.Threading.Tasks;
using Puppet.Common.StateManagement;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Puppet.Common.Devices
{
    public class Weather : DeviceBase
    {
        public IWeatherData Data;

        public Weather(IServiceProvider serviceProvider, HomeAutomationPlatform hub, string id) : base(hub, id)
        {
            Data = serviceProvider.GetService<IWeatherData>();
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
            Data.IsDaytime = GetState("is_day", Convert.ToBoolean);
            Data.SunriseTime = GetState("localSunrise", DateTime.Parse);
            Data.SunsetTime = GetState("localSunset", DateTime.Parse);
            Data.TemperatureFeelsLike = GetState("feelsLike", decimal.Parse);
            Data.TemperatureActual = GetState("temperature", decimal.Parse);
            Data.Humidity = GetState("humidity", decimal.Parse);
            Data.LastUpdated = GetState("last_poll_Forecast", DateTime.Parse);
        }
    }
}
