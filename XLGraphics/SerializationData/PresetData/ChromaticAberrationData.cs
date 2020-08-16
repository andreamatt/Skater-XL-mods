using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class ChromaticAberrationData
	{
		public bool active;
		public float intensity;
		public int maxSamples;

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
			p.chromaticAberration.maxSamples = maxSamples;
		}
	}
}
