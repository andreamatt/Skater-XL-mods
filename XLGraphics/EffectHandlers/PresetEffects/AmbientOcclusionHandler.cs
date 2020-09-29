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
	public class AmbientOcclusionHandler : PresetEffectHandler
	{
		AmbientOcclusionUI aoUI;

		public override void ConnectUI() {
			aoUI = AmbientOcclusionUI.Instance;

			// add listeners
			aoUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.active = v;
			aoUI.intensity.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.intensity.value = v;
			aoUI.directLightingStrength.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.directLightingStrength.value = v;
			aoUI.radius.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.radius.value = v;
			aoUI.quality.onValueChanged.AddListener(v => {
				PresetManager.Instance.selectedPreset.ambientOcclusion.quality.value = v;
				UpdateQualityUI(v);
			});
			aoUI.maximumRadius.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.maximumRadiusInPixels = (int)v;
			aoUI.fullResolution.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.fullResolution = v;
			aoUI.stepCount.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.stepCount = (int)v;
			aoUI.temporalAccumulation.onValueChanged += v => {
				PresetManager.Instance.selectedPreset.ambientOcclusion.temporalAccumulation.value = v;
				UpdateTemporalAccumulationUI(v);
			};
			aoUI.ghostingReduction.onValueChanged += v => PresetManager.Instance.selectedPreset.ambientOcclusion.ghostingReduction.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var ao = preset.ambientOcclusion;
			aoUI.toggle.SetIsOnWithoutNotify(ao.active);
			aoUI.intensity.OverrideValue(ao.intensity.value);
			aoUI.directLightingStrength.OverrideValue(ao.directLightingStrength.value);
			aoUI.radius.OverrideValue(ao.radius.value);
			aoUI.quality.SetValueWithoutNotify(ao.quality.value);
			aoUI.maximumRadius.OverrideValue(ao.maximumRadiusInPixels);
			aoUI.fullResolution.SetIsOnWithoutNotify(ao.fullResolution);
			aoUI.stepCount.OverrideValue(ao.stepCount);
			aoUI.temporalAccumulation.SetIsOnWithoutNotify(ao.temporalAccumulation.value);
			aoUI.ghostingReduction.OverrideValue(ao.ghostingReduction.value);

			UpdateQualityUI(ao.quality.value);
			UpdateTemporalAccumulationUI(ao.temporalAccumulation.value);
		}

		private void UpdateQualityUI(int quality) {
			var interactable = quality == (int)Quality.Custom;
			aoUI.maximumRadius.interactable = interactable;
			aoUI.fullResolution.interactable = interactable;
			aoUI.stepCount.interactable = interactable;
		}

		private void UpdateTemporalAccumulationUI(bool ta) {
			aoUI.ghostingReduction.interactable = ta;
		}
	}
}
