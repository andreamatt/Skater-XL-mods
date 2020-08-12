using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace XLGraphics.Presets.PresetData
{
	public class VignetteData
	{
		public bool active;
		// mode only Procedural because Mask requires a mask file
		//public VignetteMode mode;
		public Color color;
		// center is useless
		public float intensity;
		public float smoothness;
		public float roundness;
		public bool rounded;

		public static VignetteData FromPreset(Preset p) {
			return new VignetteData() {
				active = p.vignette.active,
				color = p.vignette.color.value,
				intensity = p.vignette.intensity.value,
				smoothness = p.vignette.smoothness.value,
				roundness = p.vignette.roundness.value,
				rounded = p.vignette.rounded.value
			};
		}

		public void OverrideValues(Preset p) {
			p.vignette.active = active;
			p.vignette.color.value = color;
			p.vignette.intensity.value = intensity;
			p.vignette.smoothness.value = smoothness;
			p.vignette.roundness.value = roundness;
			p.vignette.rounded.value = rounded;
		}
	}
}
