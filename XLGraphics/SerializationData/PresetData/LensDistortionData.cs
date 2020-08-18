using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class LensDistortionData
	{
		public bool active = false;
		public float intensity = 0.5f;
		public float xMultiplier = 1;
		public float yMultiplier = 1;
		[JsonIgnore]
		public readonly SerializableVector2 center = new Vector2(0.5f, 0.5f);
		public float scale = 1;

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
			p.lensDistortion.center.value = center;
			p.lensDistortion.scale.value = scale;
		}
	}
}
