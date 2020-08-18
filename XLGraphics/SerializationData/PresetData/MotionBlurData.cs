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
	public class MotionBlurData
	{
		public bool active = false;
		public float intensity = 0.7f;
		[JsonIgnore]
		public readonly int quality = ScalableSettingLevelParameter.LevelCount;
		public int sampleCount = 8;
		public float maximumVelocity = 200;
		public float minimumVelocity = 2;
		public float depthComparisonExtent = 1;
		public float cameraRotationVelocity = 0.03f;
		public bool cameraMotionBlur = true;

		public static MotionBlurData FromPreset(Preset p) {
			return new MotionBlurData() {
				active = p.motionBlur.active,
				intensity = p.motionBlur.intensity.value,
				sampleCount = p.motionBlur.sampleCount,
				maximumVelocity = p.motionBlur.maximumVelocity.value,
				minimumVelocity = p.motionBlur.minimumVelocity.value,
				depthComparisonExtent = p.motionBlur.depthComparisonExtent.value,
				cameraRotationVelocity = p.motionBlur.cameraRotationVelocityClamp.value,
				cameraMotionBlur = p.motionBlur.cameraMotionBlur.value
			};
		}

		public void OverrideValues(Preset p) {
			p.motionBlur.active = active;
			p.motionBlur.intensity.value = intensity;
			p.motionBlur.quality.value = quality;
			p.motionBlur.sampleCount = sampleCount;
			p.motionBlur.maximumVelocity.value = maximumVelocity;
			p.motionBlur.minimumVelocity.value = minimumVelocity;
			p.motionBlur.depthComparisonExtent.value = depthComparisonExtent;
			p.motionBlur.cameraRotationVelocityClamp.value = cameraRotationVelocity;
			p.motionBlur.cameraMotionBlur.value = cameraMotionBlur;
		}
	}
}
