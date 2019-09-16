using Puppet.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puppet.Common.Devices
{
    public enum AccelerationStatus
    {
        Active,
        Inactive,
        Unknown
    }

    public class AccelerationSensor : DeviceBase
    {
        public AccelerationSensor(HomeAutomationPlatform hub, string id) : base(hub, id)
        {
        }

        public AccelerationStatus Status
        {
            get
            {
                switch(GetState()["acceleration"])
                {
                    case "active":
                        return AccelerationStatus.Active;

                    case "inactive":
                        return AccelerationStatus.Inactive;

                    default:
                        return AccelerationStatus.Unknown;                  
                }

            }
        }
    }
}
