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
	public class VignetteHandler : PresetEffectHandler
	{

		VignetteUI vignUI;

		public override void ConnectUI() {
			vignUI = VignetteUI.Instance;

			// add listeners
			vignUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.vignette.active = v));
			vignUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => vignUI.container.SetActive(v)));
			vignUI.intensity.onValueChange += v => PresetManager.Instance.selectedPreset.vignette.intensity.value = v;
			vignUI.smoothness.onValueChange += v => PresetManager.Instance.selectedPreset.vignette.smoothness.value = v;
			vignUI.roundness.onValueChange += v => PresetManager.Instance.selectedPreset.vignette.roundness.value = v;
			vignUI.rounded.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.vignette.rounded.value = v));
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var vign = preset.vignette;
			vignUI.toggle.SetIsOnWithoutNotify(vign.active);
			vignUI.container.SetActive(vign.active);
			vignUI.intensity.OverrideValue(vign.intensity.value);
			vignUI.smoothness.OverrideValue(vign.smoothness.value);
			vignUI.roundness.OverrideValue(vign.roundness.value);
			vignUI.rounded.SetIsOnWithoutNotify(vign.rounded.value);
		}
	}
}
