using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using XLGraphics.Presets;
using XLGraphicsUI.Elements.EffectsUI;

namespace XLGraphics.Effects.PresetEffects
{
	public class DepthOfFieldHandler : PresetEffectHandler
	{

		DepthOfFieldUI dofUI;

		public override void ConnectUI() {
			dofUI = GameObject.FindObjectOfType<DepthOfFieldUI>();

			// add listeners
			dofUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.depthOfField.active = v));
			dofUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => dofUI.container.SetActive(v)));
			dofUI.focusMode.onValueChanged.AddListener(new UnityAction<int>(v => PresetManager.Instance.selectedPreset.focusMode = (FocusMode)v));
			dofUI.focusDistance.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.focusDistance.value = v;
			dofUI.nearFocusStart.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.nearFocusStart.value = v;
			dofUI.nearFocusEnd.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.nearFocusEnd.value = v;
			dofUI.farFocusStart.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.farFocusStart.value = v;
			dofUI.farFocusEnd.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.farFocusEnd.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var dof = preset.depthOfField;
			dofUI.toggle.SetIsOnWithoutNotify(dof.active);
			dofUI.container.SetActive(dof.active);
			dofUI.focusMode.SetValueWithoutNotify((int)preset.focusMode);
			dofUI.focusDistance.OverrideValue(dof.focusDistance.value);
			dofUI.nearFocusStart.OverrideValue(dof.nearFocusStart.value);
			dofUI.nearFocusEnd.OverrideValue(dof.nearFocusEnd.value);
			dofUI.farFocusStart.OverrideValue(dof.farFocusStart.value);
			dofUI.farFocusEnd.OverrideValue(dof.farFocusEnd.value);
		}
	}

	public enum FocusMode
	{
		PhysicalCamera,
		Player,
		Skate,
		Manual
	}
}
