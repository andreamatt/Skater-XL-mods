using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class MotionBlurData
	{
		public bool active;
		public float intensity;
		// it could have quality. Just set it to custom with good default values
		//public int quality;
		public int sampleCount;
		public float maximumVelocity;
		public float minimumVelocity;
		public float depthComparisonExtent;
		public float cameraRotationVelocity;
		public bool cameraMotionBlur;

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
			p.motionBlur.sampleCount = sampleCount;
			p.motionBlur.maximumVelocity.value = maximumVelocity;
			p.motionBlur.minimumVelocity.value = minimumVelocity;
			p.motionBlur.depthComparisonExtent.value = depthComparisonExtent;
			p.motionBlur.cameraRotationVelocityClamp.value = cameraRotationVelocity;
			p.motionBlur.cameraMotionBlur.value = cameraMotionBlur;
		}
	}
}
