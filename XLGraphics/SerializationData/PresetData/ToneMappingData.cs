using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class ToneMappingData
	{
		public bool active;
		public TonemappingMode mode; // external not supported
		public float toeStrength;
		public float toeLength;
		public float shoulderStrength;
		public float shoulderLength;
		public float shoulderAngle;
		public float gamma;

		public static ToneMappingData FromPreset(Preset p) {
			return new ToneMappingData() {
				active = p.tonemapping.active,
				mode = p.tonemapping.mode.value,
				toeStrength = p.tonemapping.toeStrength.value,
				toeLength = p.tonemapping.toeLength.value,
				shoulderStrength = p.tonemapping.shoulderStrength.value,
				shoulderLength = p.tonemapping.shoulderLength.value,
				shoulderAngle = p.tonemapping.shoulderAngle.value,
				gamma = p.tonemapping.gamma.value,
			};
		}

		public void OverrideValues(Preset p) {
			p.tonemapping.active = active;
			p.tonemapping.mode.value = mode;
			p.tonemapping.toeStrength.value = toeStrength;
			p.tonemapping.toeLength.value = toeLength;
			p.tonemapping.shoulderStrength.value = shoulderStrength;
			p.tonemapping.shoulderLength.value = shoulderLength;
			p.tonemapping.shoulderAngle.value = shoulderAngle;
			p.tonemapping.gamma.value = gamma;
		}
	}
}
