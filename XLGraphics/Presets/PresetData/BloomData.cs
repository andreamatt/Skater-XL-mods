using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class BloomData
	{
		public float intensity;
		public float scatter;
		public float threshold;

		public static BloomData FromPreset(Preset p) {
			return new BloomData() {
				intensity = p.bloom.intensity.value,
				scatter = p.bloom.scatter.value,
				threshold = p.bloom.threshold.value
			};
		}

		public void OverrideValues(Preset p) {
			p.bloom.intensity.value = intensity;
			p.bloom.scatter.value = scatter;
			p.bloom.threshold.value = threshold;
		}
	}
}
