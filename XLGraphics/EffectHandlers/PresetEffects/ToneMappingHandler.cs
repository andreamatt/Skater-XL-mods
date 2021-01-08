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
	public class ToneMappingHandler : PresetEffectHandler
	{
		ToneMappingUI tmUI;

		public override void ConnectUI() {
			tmUI = ToneMappingUI.Instance;

			// add listeners
			tmUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.active = v;
			tmUI.mode.onValueChanged.AddListener(v => {
				PresetManager.Instance.selectedPreset.tonemapping.mode.value = (TonemappingMode)v;
				UpdateModeUI((TonemappingMode)v);
			});
			tmUI.toeStrength.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.toeStrength.value = v;
			tmUI.toeLength.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.toeStrength.value = v;
			tmUI.shoulderStrength.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.shoulderStrength.value = v;
			tmUI.shoulderLength.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.shoulderLength.value = v;
			tmUI.shoulderAngle.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.shoulderAngle.value = v;
			tmUI.gamma.onValueChanged += v => PresetManager.Instance.selectedPreset.tonemapping.gamma.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var tm = preset.tonemapping;
			tmUI.toggle.SetIsOnWithoutNotify(tm.active);
			tmUI.mode.SetValueWithoutNotify((int)tm.mode.value);
			tmUI.toeStrength.OverrideValue(tm.toeStrength.value);
			tmUI.toeLength.OverrideValue(tm.toeLength.value);
			tmUI.shoulderStrength.OverrideValue(tm.shoulderStrength.value);
			tmUI.shoulderLength.OverrideValue(tm.shoulderLength.value);
			tmUI.shoulderAngle.OverrideValue(tm.shoulderAngle.value);
			tmUI.gamma.OverrideValue(tm.gamma.value);

			UpdateModeUI(tm.mode.value);
		}

		private void UpdateModeUI(TonemappingMode mode) {
			var interactable = mode == TonemappingMode.Custom;
			tmUI.toeStrength.interactable = interactable;
			tmUI.toeLength.interactable = interactable;
			tmUI.shoulderStrength.interactable = interactable;
			tmUI.shoulderLength.interactable = interactable;
			tmUI.shoulderAngle.interactable = interactable;
			tmUI.gamma.interactable = interactable;
		}
	}
}
