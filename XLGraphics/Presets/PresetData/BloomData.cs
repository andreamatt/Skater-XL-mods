using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.Presets.PresetData
{
	public class BloomData
	{
		public bool active;
		public float intensity;
		public float scatter;
		public float threshold;
		public SerializableColor tint;

		public static BloomData FromPreset(Preset p) {
			return new BloomData() {
				active = p.bloom.active,
				intensity = p.bloom.intensity.value,
				scatter = p.bloom.scatter.value,
				threshold = p.bloom.threshold.value,
				tint = p.bloom.tint.value
			};
		}

		public void OverrideValues(Preset p) {
			p.bloom.active = active;
			p.bloom.intensity.value = intensity;
			p.bloom.scatter.value = scatter;
			p.bloom.threshold.value = threshold;
			p.bloom.tint.value = tint;
		}
	}
}
