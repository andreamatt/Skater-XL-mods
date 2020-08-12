using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class ChannelMixerData
	{
		public bool active;
		public float intensity;
		public float scatter;
		public float threshold;
		public float redOutRedIn;
		public float redOutGreenIn;
		public float redOutBlueIn;
		public float greenOutRedIn;
		public float greenOutGreenIn;
		public float greenOutBlueIn;
		public float blueOutRedIn;
		public float blueOutGreenIn;
		public float blueOutBlueIn;

		public static ChannelMixerData FromPreset(Preset p) {
			return new ChannelMixerData() {
				active = p.channelMixer.active,
				redOutRedIn = p.channelMixer.redOutRedIn.value,
				redOutGreenIn = p.channelMixer.redOutGreenIn.value,
				redOutBlueIn = p.channelMixer.redOutBlueIn.value,
				greenOutRedIn = p.channelMixer.greenOutRedIn.value,
				greenOutGreenIn = p.channelMixer.greenOutGreenIn.value,
				greenOutBlueIn = p.channelMixer.greenOutBlueIn.value,
				blueOutRedIn = p.channelMixer.blueOutRedIn.value,
				blueOutGreenIn = p.channelMixer.blueOutGreenIn.value,
				blueOutBlueIn = p.channelMixer.blueOutBlueIn.value,
			};
		}

		public void OverrideValues(Preset p) {
			p.channelMixer.active = active;
			p.channelMixer.redOutRedIn.value = redOutRedIn;
			p.channelMixer.redOutGreenIn.value = redOutGreenIn;
			p.channelMixer.redOutBlueIn.value = redOutBlueIn;
			p.channelMixer.greenOutRedIn.value = greenOutRedIn;
			p.channelMixer.greenOutGreenIn.value = greenOutGreenIn;
			p.channelMixer.greenOutBlueIn.value = greenOutBlueIn;
			p.channelMixer.blueOutRedIn.value = blueOutRedIn;
			p.channelMixer.blueOutGreenIn.value = blueOutGreenIn;
			p.channelMixer.blueOutBlueIn.value = blueOutBlueIn;
		}
	}
}
