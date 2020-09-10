using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using XLGraphics.Presets;
using XLGraphicsUI.Elements.EffectsUI;

namespace XLGraphics.EffectHandlers.PresetEffects
{
	public class LightHandler : PresetEffectHandler
	{
		LightUI lightUI;

		public override void ConnectUI() {
			lightUI = LightUI.Instance;

			// add listeners
			lightUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.lightData.active = v));
			lightUI.intensity.onValueChange += v => PresetManager.Instance.selectedPreset.lightData.intensity = v;
			lightUI.range.onValueChange += v => PresetManager.Instance.selectedPreset.lightData.range = v;
			lightUI.angle.onValueChange += v => PresetManager.Instance.selectedPreset.lightData.angle = v;
			// color..
			lightUI.position.onValueChange += v => PresetManager.Instance.selectedPreset.lightData.position = v;
			// cookie..
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var light = preset.lightData;
			lightUI.toggle.SetIsOnWithoutNotify(light.active);
			lightUI.intensity.OverrideValue(light.intensity);
			lightUI.range.OverrideValue(light.range);
			lightUI.angle.OverrideValue(light.angle);
			// color..
			lightUI.position.OverrideValue(light.position);
			// cookie..
		}
	}
}
