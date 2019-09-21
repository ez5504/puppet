using Puppet.Common.Automation;
using Puppet.Common.Devices;
using Puppet.Common.Events;
using Puppet.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puppet.Automation
{
    [TriggerDevice("Button.LivingRoomRemote", Capability.Pushed)]
    public class LivingRoomRemoteControl : AutomationBase
    {
        DimmerSwitchRelay _livingRoomLight;
        LockDevice _frontDoorLock;
        Speaker _alexaLivingRoom;
        Weather _weather;

        public LivingRoomRemoteControl(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {
        }

        protected override async Task Handle()
        {
            if(_evt.IsButtonPushedEvent)
            {
                var brightness = _livingRoomLight.Brightness;
                var currTime = DateTime.Now;
                switch(_evt.Value)
                {
                    case "1":
                        await _livingRoomLight.On();
                        if(currTime.Hour > 5 && currTime.Hour < 8)
                        {
                            await _livingRoomLight.SetBrightness(80);
                            await _livingRoomLight.SetColor(12, 80);
                            await _alexaLivingRoom.Speak($"Good morning Jacob. The current forecast is {_weather.CurrentCondition()}. The temperature outside is {_weather.TemperatureFeelsLike()}.");
                        }
                        else 
                        {
                            await _livingRoomLight.SetBrightness(100);
                        }
                        break;
                    case "2":
                        if(brightness <= 80) {
                            await _livingRoomLight.SetBrightness(brightness + 20);
                        }
                        else {
                            await _livingRoomLight.SetBrightness(100);
                        }
                        break;
                    case "3":
                        if(brightness > 20) {
                            await _livingRoomLight.SetBrightness(brightness - 20);
                        }
                        else if (brightness  > 10) {
                            await _livingRoomLight.SetBrightness(brightness - 10);
                        }
                        else {
                            await _livingRoomLight.SetBrightness(1);
                        }
                        break;
                    case "4":
                        await _livingRoomLight.Off();
                        if(currTime.Hour > 6 && currTime.Hour < 8)
                        {
                            await _frontDoorLock.Unlock();
                        }
                        break;
                }
            }
        }
        protected override async Task InitDevices()
        {
            _livingRoomLight = await _hub.GetDeviceByMappedName<DimmerSwitchRelay>("Switch.LivingRoom");
            _frontDoorLock = await _hub.GetDeviceByMappedName<LockDevice>("Lock.FrontDoorDeadbolt");
            _alexaLivingRoom = await _hub.GetDeviceByMappedName<Speaker>("Speaker.Alexa-LivingRoom");
            _weather = await _hub.GetDeviceByMappedName<Weather>("Misc.Weather");
        }
    }
}
