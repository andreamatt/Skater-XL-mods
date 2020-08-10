using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class ChromaticAberrationData
	{
		public float intensity;
		public int maxSamples;

		public static ChromaticAberrationData FromPreset(Preset p) {
			return new ChromaticAberrationData() {
				intensity = p.chromaticAberration.intensity.value,
				maxSamples = p.chromaticAberration.maxSamples
			};
		}

		public void OverrideValues(Preset p) {
			p.chromaticAberration.intensity.value = intensity;
			p.chromaticAberration.maxSamples = maxSamples;
		}
	}
}
