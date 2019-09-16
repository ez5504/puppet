using System;
using System.Threading.Tasks;
using Puppet.Common.Automation;
using Puppet.Common.Devices;
using Puppet.Common.Events;
using Puppet.Common.Services;

namespace Puppet.Automation
{
    [TriggerDevice("Contact.Washer", Capability.Contact)]
    public class LaundryLightsAutomation : AutomationBase
    {
        SwitchRelay _livingRoom2;

        public LaundryLightsAutomation(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        {            
        }

        protected override async Task InitDevices()
        {
            _livingRoom2 = await _hub.GetDeviceByMappedName<SwitchRelay>("Switch.LivingRoom2");
        }

        protected override async Task Handle() 
        {
            await WaitForCancellationAsync(TimeSpan.FromSeconds(5));
            Console.WriteLine("Washing Machine");
        }
    }
}