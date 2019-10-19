using Newtonsoft.Json.Linq;
using Puppet.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Puppet.Common.Devices
{
    public abstract class DeviceBase : IDevice
    {
        internal HomeAutomationPlatform _hub;
        internal Dictionary<string, string> _state;

        public string Id { get; }
        public string Name => GetState()["name"]; 
        public string Label => GetState()["label"];
            
        public DeviceBase(HomeAutomationPlatform hub, string id)
        {
            _hub = hub;
            Id = id;
        }
        
        public async Task RefreshState()
        {
            _state = await _hub.GetDeviceState(this);
        }

        internal Dictionary<string, string> GetState()
        {
            if(_state == null)
            {
                Task.Run(() => RefreshState()).Wait(); 
            }
            return _state;
        }

        internal T GetState<T>(string key, Func<string, T> conversion = null) 
        {
            if(GetState().TryGetValue(key, out var value))
            {
                return conversion == null ? value : conversion(value);
            }
            return default(T);
        }

        public async Task DoAction(string command, string parameter = null)
        {
            string[] args = null;
            if (parameter != null)
            {
                args = new string[] { parameter };
            }
            await _hub.DoAction(this, command, args);
        }
    }
}
