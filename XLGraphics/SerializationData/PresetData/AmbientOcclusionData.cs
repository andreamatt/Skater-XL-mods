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
	public class AmbientOcclusionData
	{
		public bool active = false;
		public float intensity = 1.9f;
		public float directLightingStrength = 1.9f;
		public float radius = 2;
		public Quality quality = Quality.Custom;
		public int maximumRadius = 60;
		public bool fullResolution = true;
		public int stepCount = 60;
		public bool temporalAccumulation = false;
		public float ghostingReduction = 0.5f;

		public static AmbientOcclusionData FromPreset(Preset p) {
			return new AmbientOcclusionData() {
				active = p.ambientOcclusion.active,
				intensity = p.ambientOcclusion.intensity.value,
				directLightingStrength = p.ambientOcclusion.directLightingStrength.value,
				radius = p.ambientOcclusion.radius.value,
				quality = (Quality)p.ambientOcclusion.quality.value,
				maximumRadius = p.ambientOcclusion.maximumRadiusInPixels,
				fullResolution = p.ambientOcclusion.fullResolution,
				stepCount = p.ambientOcclusion.stepCount,
				temporalAccumulation = p.ambientOcclusion.temporalAccumulation.value,
				ghostingReduction = p.ambientOcclusion.ghostingReduction.value
			};
		}

		public void OverrideValues(Preset p) {
			p.ambientOcclusion.active = active;
			p.ambientOcclusion.intensity.value = intensity;
			p.ambientOcclusion.directLightingStrength.value = directLightingStrength;
			p.ambientOcclusion.radius.value = radius;
			p.ambientOcclusion.quality.value = (int)quality;
			p.ambientOcclusion.maximumRadiusInPixels = maximumRadius;
			p.ambientOcclusion.fullResolution = fullResolution;
			p.ambientOcclusion.stepCount = stepCount;
			p.ambientOcclusion.temporalAccumulation.value = temporalAccumulation;
			p.ambientOcclusion.ghostingReduction.value = ghostingReduction;
		}
	}
}
