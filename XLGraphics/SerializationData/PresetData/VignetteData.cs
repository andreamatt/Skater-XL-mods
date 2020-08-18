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
	public class VignetteData
	{
		public bool active;
		// mode only Procedural because Mask requires a mask file
		[JsonIgnore]
		public readonly VignetteMode mode = VignetteMode.Procedural;
		public SerializableColor color = Color.black;
		[JsonIgnore]
		public readonly SerializableVector2 center = new Vector2(0.5f, 0.5f);
		public float intensity = 0.35f;
		public float smoothness = 0.2f;
		public float roundness = 1;
		public bool rounded = true;

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
			p.vignette.mode.value = mode;
			p.vignette.color.value = color;
			p.vignette.intensity.value = intensity;
			p.vignette.smoothness.value = smoothness;
			p.vignette.roundness.value = roundness;
			p.vignette.rounded.value = rounded;
		}
	}
}
