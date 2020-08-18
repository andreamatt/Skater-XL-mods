using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class ChromaticAberrationData
	{
		public bool active = false;
		public float intensity = 0.15f;
		// no spectral lut (requires texture)
		// quality needs to be custom to use maxSamples
		// to set custom quality, set it quality = number of quality levels
		[JsonIgnore]
		public readonly int quality = ScalableSettingLevelParameter.LevelCount;
		public int maxSamples = 6;

		public static ChromaticAberrationData FromPreset(Preset p) {
			return new ChromaticAberrationData() {
				active = p.chromaticAberration.active,
				intensity = p.chromaticAberration.intensity.value,
				maxSamples = p.chromaticAberration.maxSamples
			};
		}

		public void OverrideValues(Preset p) {
			p.chromaticAberration.active = active;
			p.chromaticAberration.intensity.value = intensity;
			p.chromaticAberration.quality.value = quality;
			p.chromaticAberration.maxSamples = maxSamples;
		}
	}
}
