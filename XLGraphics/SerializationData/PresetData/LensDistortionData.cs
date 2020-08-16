using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class LensDistortionData
	{
		public bool active;
		public float intensity;
		public float xMultiplier;
		public float yMultiplier;
		public float scale;

		public static LensDistortionData FromPreset(Preset p) {
			return new LensDistortionData() {
				active = p.lensDistortion.active,
				intensity = p.lensDistortion.intensity.value,
				xMultiplier = p.lensDistortion.xMultiplier.value,
				yMultiplier = p.lensDistortion.yMultiplier.value,
				scale = p.lensDistortion.scale.value
			};
		}

		public void OverrideValues(Preset p) {
			p.lensDistortion.active = active;
			p.lensDistortion.intensity.value = intensity;
			p.lensDistortion.xMultiplier.value = xMultiplier;
			p.lensDistortion.yMultiplier.value = yMultiplier;
			p.lensDistortion.scale.value = scale;
		}
	}
}
