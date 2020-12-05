using Puppet.Common.Automation;
using Puppet.Common.Devices;
using Puppet.Common.Events;
using Puppet.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Puppet.Automation
{
    [TriggerDevice("Lock.FrontDoorDeadbolt", Capability.Lock)]
    public class LockFrontDoor : AutomationBase
    {
        LockDevice _frontDoorLock;

        DimmerSwitchRelay _livingRoomLight;

        Weather _weather;

        public LockFrontDoor(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {}

        protected override async Task InitDevices()
        {
            _frontDoorLock =
                await _hub.GetDeviceByMappedName<LockDevice>("Lock.FrontDoorDeadbolt");

            _livingRoomLight =
                await _hub.GetDeviceByMappedName<DimmerSwitchRelay>("Switch.LivingRoom");
            
            _weather = await _hub.GetDeviceByMappedName<Weather>("Misc.Weather");
        }

        protected override async Task Handle()
        {
            if(_frontDoorLock.Status == LockStatus.Unlocked) 
            {
                DateTime sunsetTime = await _weather.SunsetTime();
                var currTime = DateTime.Now;
                if(currTime > (sunsetTime.AddHours(-1.5)))
                {
                    await _livingRoomLight.On();
                    await _livingRoomLight.SetBrightness(100);
                }
            }
        }
    }
}
