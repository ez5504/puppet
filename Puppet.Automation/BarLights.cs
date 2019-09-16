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
    [TriggerDevice("Contact.BarDoor1", Capability.Contact)]
    public class BarLights : AutomationBase
    {
        SwitchRelay _barOutlet;
        
        public BarLights(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {}

        protected override async Task InitDevices()
        {
            _barOutlet =
                await _hub.GetDeviceByMappedName<SwitchRelay>("Outlet.Bar");
        }

        protected override async Task Handle()
        {
            if(_evt.IsOpenEvent)
            {
                await _barOutlet.On();
            }
            else
            {
                await _barOutlet.Off();
            }
        }
    }
}
