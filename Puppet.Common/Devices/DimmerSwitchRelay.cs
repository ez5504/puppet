
using Puppet.Common.Services;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Puppet.Common.Devices
{
    public class DimmerSwitchRelay : SwitchRelay
    {

        public DimmerSwitchRelay(HomeAutomationPlatform hub, string id) : base(hub, id)
        {
        }

        public int Brightness => int.Parse(GetState()["level"]);

        public int Hue => int.Parse(GetState()["hue"]);

        public int Saturation => int.Parse(GetState()["saturation"]);

        public KeyValuePair<int, int> Color
        {
            get
            {
                return new KeyValuePair<int, int>(this.Hue, this.Saturation);
            }
        }

        public async Task SetBrightness(int value)
        {
            await this.DoAction("setLevel", value.ToString());
        }

        public async Task SetColor(int hue, int saturation)
        {
            await this.SetHue(hue);
            await this.SetSaturation(saturation);
        }

        public async Task SetHue(int value)
        {
            await this.DoAction("setHue", value.ToString());
        }

        public async Task SetSaturation(int value)
        {
            await this.DoAction("setSaturation", value.ToString());
        }
    }
}
