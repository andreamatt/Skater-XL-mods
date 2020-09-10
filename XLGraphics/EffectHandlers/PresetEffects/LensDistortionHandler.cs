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
	public class LensDistortionHandler : PresetEffectHandler
	{

		LensDistortionUI ldUI;

		public override void ConnectUI() {
			ldUI = LensDistortionUI.Instance;

			// add listeners
			ldUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.lensDistortion.active = v));
			ldUI.intensity.onValueChange += v => PresetManager.Instance.selectedPreset.lensDistortion.intensity.value = v;
			ldUI.xMultiplier.onValueChange += v => PresetManager.Instance.selectedPreset.lensDistortion.xMultiplier.value = v;
			ldUI.yMultiplier.onValueChange += v => PresetManager.Instance.selectedPreset.lensDistortion.yMultiplier.value = v;
			ldUI.scale.onValueChange += v => PresetManager.Instance.selectedPreset.lensDistortion.scale.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var lens = preset.lensDistortion;
			ldUI.toggle.SetIsOnWithoutNotify(lens.active);
			ldUI.intensity.OverrideValue(lens.intensity.value);
			ldUI.xMultiplier.OverrideValue(lens.xMultiplier.value);
			ldUI.yMultiplier.OverrideValue(lens.yMultiplier.value);
			ldUI.scale.OverrideValue(lens.scale.value);
		}
	}
}
