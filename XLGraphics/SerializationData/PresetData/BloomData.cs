using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class BloomData
	{
		public bool active = false;
		[JsonIgnore]
		public readonly int quality = ScalableSettingLevelParameter.LevelCount;
		public float intensity = 0.15f;
		public float scatter = 0.7f;
		public float threshold = 0;
		public SerializableColor tint = Color.white;
		// no lens dirt (requires texture)

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
			p.bloom.quality.value = quality;
			p.bloom.intensity.value = intensity;
			p.bloom.scatter.value = scatter;
			p.bloom.threshold.value = threshold;
			p.bloom.tint.value = tint;
		}
	}
}
