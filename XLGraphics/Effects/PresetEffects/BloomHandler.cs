using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI.Elements;

namespace XLGraphics.Effects.PresetEffects
{
	public class BloomHandler : PresetEffectHandler
	{
		XLToggle enabledToggle;
		XLSlider intensitySlider;
		XLSlider scatterSlider;
		XLSlider thresholdSlider;

		public override void ConnectUI() {
			enabledToggle = UI.Instance.toggles["BloomToggle"];
			intensitySlider = UI.Instance.sliders["BloomIntensitySlider"];
			scatterSlider = UI.Instance.sliders["BloomScatterSlider"];
			thresholdSlider = UI.Instance.sliders["BloomThresholdSlider"];

			// add listeners
			enabledToggle.ValueChange += v => PresetManager.Instance.selectedPreset.bloom.active = v;
			intensitySlider.ValueChange += v => PresetManager.Instance.selectedPreset.bloom.intensity.value = v;
			scatterSlider.ValueChange += v => PresetManager.Instance.selectedPreset.bloom.scatter.value = v;
			thresholdSlider.ValueChange += v => PresetManager.Instance.selectedPreset.bloom.threshold.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var ca = preset.bloom;
			enabledToggle.OverrideValue(ca.active);
			intensitySlider.OverrideValue(ca.intensity.value);
			scatterSlider.OverrideValue(ca.intensity.value);
			thresholdSlider.OverrideValue(ca.intensity.value);
		}
	}
}
