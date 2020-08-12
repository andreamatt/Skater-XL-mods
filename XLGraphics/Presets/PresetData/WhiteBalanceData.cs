using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class WhiteBalanceData
	{
		public bool active;
		public float temperature;
		public float tint;

		public static WhiteBalanceData FromPreset(Preset p) {
			return new WhiteBalanceData() {
				active = p.whiteBalance.active,
				temperature = p.whiteBalance.temperature.value,
				tint = p.whiteBalance.tint.value
			};
		}

		public void OverrideValues(Preset p) {
			p.whiteBalance.active = active;
			p.whiteBalance.temperature.value = temperature;
			p.whiteBalance.tint.value = tint;
		}
	}
}
