using System.Threading.Tasks;
using Puppet.Common.Automation;
using Puppet.Common.Devices;
using Puppet.Common.Events;
using Puppet.Common.Services;
using System;

namespace Puppet.Automation
{
    [TriggerDevice("Switch.LivingRoom2", Capability.Switch)]
    public class LivingRoomLightAutomation : AutomationBase
    {
        SwitchRelay _livingRoom2;
        public LivingRoomLightAutomation(HomeAutomationPlatform hub, HubEvent evt) : base(hub, evt)
        { }

        protected override async Task InitDevices()
        {
            _livingRoom2 =
                await _hub.GetDeviceByMappedName<SwitchRelay>("Switch.LivingRoom2");
        }

        protected override async Task Handle()
        {
            if(_evt.IsOnEvent)
            {
                await WaitForCancellationAsync(TimeSpan.FromSeconds(30));
                await _livingRoom2.Off();
            }
        }
    }
}