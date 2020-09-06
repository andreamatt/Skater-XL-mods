using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using XLGraphics.Presets;
using XLGraphicsUI.Elements.EffectsUI;

namespace XLGraphics.EffectHandlers.PresetEffects
{
	public class MotionBlurHandler : PresetEffectHandler
	{

		MotionBlurUI mbUI;

		public override void ConnectUI() {
			mbUI = MotionBlurUI.Instance;

			// add listeners
			mbUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.motionBlur.active = v));
			mbUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => mbUI.container.SetActive(v)));
			mbUI.intensity.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.intensity.value = v;
			mbUI.sampleCount.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.sampleCount = (int)v;
			mbUI.maximumVelocity.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.maximumVelocity.value = v;
			mbUI.minimumVelocity.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.minimumVelocity.value = v;
			mbUI.depthComparisonExtent.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.depthComparisonExtent.value = v;
			mbUI.cameraRotationVelocityClamp.onValueChange += v => PresetManager.Instance.selectedPreset.motionBlur.cameraRotationVelocityClamp.value = v;
			mbUI.cameraMotionBlur.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.motionBlur.cameraMotionBlur.value = v));
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var blur = preset.motionBlur;
			mbUI.toggle.SetIsOnWithoutNotify(blur.active);
			mbUI.container.SetActive(blur.active);
			mbUI.intensity.OverrideValue(blur.intensity.value);
			mbUI.sampleCount.OverrideValue(blur.sampleCount);
			mbUI.maximumVelocity.OverrideValue(blur.maximumVelocity.value);
			mbUI.minimumVelocity.OverrideValue(blur.minimumVelocity.value);
			mbUI.depthComparisonExtent.OverrideValue(blur.depthComparisonExtent.value);
			mbUI.cameraRotationVelocityClamp.OverrideValue(blur.cameraRotationVelocityClamp.value);
			mbUI.cameraMotionBlur.SetIsOnWithoutNotify(blur.cameraMotionBlur.value);
		}
	}
}
