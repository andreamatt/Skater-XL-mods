using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;
using XLGraphicsUI.Elements.EffectsUI;

namespace XLGraphics.EffectHandlers.PresetEffects
{
	public class DepthOfFieldHandler : PresetEffectHandler
	{

		DepthOfFieldUI dofUI;

		public override void ConnectUI() {
			dofUI = DepthOfFieldUI.Instance;

			// add listeners
			dofUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.depthOfField.active = v));
			dofUI.focusMode.onValueChanged.AddListener(new UnityAction<int>(v => {
				PresetManager.Instance.selectedPreset.focusMode = (FocusMode)v;
				if (v == (int)FocusMode.PhysicalCamera) {
					PresetManager.Instance.selectedPreset.depthOfField.focusMode.value = DepthOfFieldMode.UsePhysicalCamera;
				}
				else if (v == (int)FocusMode.Manual) {
					PresetManager.Instance.selectedPreset.depthOfField.focusMode.value = DepthOfFieldMode.Manual;
				}
				else {
					PresetManager.Instance.selectedPreset.depthOfField.focusMode.value = DepthOfFieldMode.Off;
				}
				UpdateActiveSliders((FocusMode)v);
				// other focusModes use dofmode.off but it gets set in customdofcontroller updatedofmode
				//CustomDofController.Instance.UpdateDofMode();
			}));
			dofUI.focusDistance.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.focusDistance.value = v;
			dofUI.nearFocusStart.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.nearFocusStart.value = v;
			dofUI.nearFocusEnd.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.nearFocusEnd.value = v;
			dofUI.farFocusStart.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.farFocusStart.value = v;
			dofUI.farFocusEnd.onValueChange += v => PresetManager.Instance.selectedPreset.depthOfField.farFocusEnd.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var dof = preset.depthOfField;
			dofUI.toggle.SetIsOnWithoutNotify(dof.active);
			dofUI.focusMode.SetValueWithoutNotify((int)preset.focusMode);
			dofUI.focusDistance.OverrideValue(dof.focusDistance.value);
			dofUI.nearFocusStart.OverrideValue(dof.nearFocusStart.value);
			dofUI.nearFocusEnd.OverrideValue(dof.nearFocusEnd.value);
			dofUI.farFocusStart.OverrideValue(dof.farFocusStart.value);
			dofUI.farFocusEnd.OverrideValue(dof.farFocusEnd.value);

			UpdateActiveSliders(preset.focusMode);
		}

		private void UpdateActiveSliders(FocusMode focusMode) {
			dofUI.focusDistance.gameObject.SetActive(false);
			dofUI.nearFocusStart.gameObject.SetActive(false);
			dofUI.nearFocusEnd.gameObject.SetActive(false);
			dofUI.farFocusStart.gameObject.SetActive(false);
			dofUI.farFocusEnd.gameObject.SetActive(false);

			if (focusMode == FocusMode.Manual) {
				dofUI.nearFocusStart.gameObject.SetActive(true);
				dofUI.nearFocusEnd.gameObject.SetActive(true);
				dofUI.farFocusStart.gameObject.SetActive(true);
				dofUI.farFocusEnd.gameObject.SetActive(true);
			}
			else if (focusMode == FocusMode.PhysicalCamera) {
				dofUI.focusDistance.gameObject.SetActive(true);
			}
		}
	}

	public enum FocusMode
	{
		Off,
		PhysicalCamera,
		Player,
		Skate,
		Manual
	}
}
