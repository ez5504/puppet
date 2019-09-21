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
    [TriggerDevice("Switch.LivingRoom", Capability.Contact)]
    public class BedroomLights : AutomationBase
    {
        DimmerSwitchRelay _bedroomLights;
        
        public BedroomLights(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {}

        protected override async Task InitDevices()
        {
            _bedroomLights =
                await _hub.GetDeviceByMappedName<DimmerSwitchRelay>("Switch.Bedroom");
        }

        protected override async Task Handle()
        {
            if(_evt.IsOffEvent && IsBedtime)
            {
                await _bedroomLights.On();
            }
        }

        protected bool IsBedtime => DateTime.Now.Hour > 21 || DateTime.Now.Hour <= 2;
    }
}
