using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI.Elements;
using XLGraphicsUI.Elements.EffectsUI;

namespace XLGraphics.EffectHandlers.PresetEffects
{
	public class ColorAdjustmentsHandler : PresetEffectHandler
	{
		ColorAdjustementsUI caUI;

		public override void ConnectUI() {
			caUI = ColorAdjustementsUI.Instance;

			// add listeners
			caUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.colorAdjustments.active = v;
			caUI.postExposure.onValueChanged += v => PresetManager.Instance.selectedPreset.colorAdjustments.postExposure.value = v;
			caUI.contrast.onValueChanged += v => PresetManager.Instance.selectedPreset.colorAdjustments.contrast.value = v;
			caUI.colorFilter.onValueChanged += v => {
				PresetManager.Instance.selectedPreset.colorAdjustmentsData.colorFilter = new Color(v.x, v.y, v.z);
				UpdateColor();
			};
			caUI.colorFilterIntensity.onValueChanged += v => {
				PresetManager.Instance.selectedPreset.colorAdjustmentsData.colorFilterIntensity = v;
				UpdateColor();
			};
			caUI.hueShift.onValueChanged += v => PresetManager.Instance.selectedPreset.colorAdjustments.hueShift.value = v;
			caUI.saturation.onValueChanged += v => PresetManager.Instance.selectedPreset.colorAdjustments.saturation.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var ca = preset.colorAdjustments;
			caUI.toggle.SetIsOnWithoutNotify(ca.active);
			caUI.postExposure.OverrideValue(ca.postExposure.value);
			caUI.contrast.OverrideValue(ca.contrast.value);
			var color = preset.colorAdjustmentsData.colorFilter;
			caUI.colorFilter.OverrideValue(new Vector3(color.r, color.g, color.b));
			caUI.colorFilterIntensity.OverrideValue(preset.colorAdjustmentsData.colorFilterIntensity);
			caUI.hueShift.OverrideValue(ca.hueShift.value);
			caUI.saturation.OverrideValue(ca.saturation.value);

			UpdateColor();
		}

		private void UpdateColor() {
			PresetManager.Instance.selectedPreset.colorAdjustments.colorFilter.value = PresetManager.Instance.selectedPreset.colorAdjustmentsData.GetFinalColor();
		}
	}
}
