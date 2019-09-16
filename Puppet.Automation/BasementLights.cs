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
    [TriggerDevice("Motion.Basement", Capability.Motion)]
    public class BasementLights : AutomationBase
    {
        SwitchRelay _basementFurnace;
        SwitchRelay _basementWasher;
        
        public BasementLights(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {}

        protected override async Task InitDevices()
        {
            _basementFurnace =
                await _hub.GetDeviceByMappedName<SwitchRelay>("Switch.BasementFurnace");
            _basementWasher =
                await _hub.GetDeviceByMappedName<SwitchRelay>("Switch.BasementWasher");
        }

        protected override async Task Handle()
        {
            if(_evt.IsActiveEvent && !LightsOn)
            {
                await _basementFurnace.On();
                await _basementWasher.On();
                await WaitForCancellationAsync(TimeSpan.FromMinutes(10));
                await TurnLightsOff();
            }
            else if(_evt.IsActiveEvent && LightsOn)
            {
                await WaitForCancellationAsync(TimeSpan.FromSeconds(30));
                await TurnLightsOff();
            }
        }

        private async Task TurnLightsOff() 
        {
            await _basementFurnace.Off();
            await _basementWasher.Off();
        }

        private bool LightsOn =>  _basementFurnace.Status == SwitchStatus.On && _basementWasher.Status == SwitchStatus.On;
    }
}
