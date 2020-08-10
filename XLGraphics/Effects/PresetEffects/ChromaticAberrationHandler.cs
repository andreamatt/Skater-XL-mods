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
	public class ChromaticAberrationHandler : PresetEffectHandler
	{
		XLToggle enabledToggle;
		XLSlider intensitySlider;
		XLSlider samplesSlider;

		public override void ConnectUI() {
			enabledToggle = UI.Instance.toggles["ChromaticAberrationToggle"];
			intensitySlider = UI.Instance.sliders["ChromaticAberrationIntensitySlider"];
			samplesSlider = UI.Instance.sliders["ChromaticAberrationSamplesSlider"];

			// add listeners
			enabledToggle.ValueChanged += v => PresetManager.Instance.selectedPreset.chromaticAberration.active = v;
			intensitySlider.ValueChanged += v => PresetManager.Instance.selectedPreset.chromaticAberration.intensity.value = v;
			samplesSlider.ValueChanged += v => PresetManager.Instance.selectedPreset.chromaticAberration.maxSamples = (int)Math.Round(v);
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var ca = preset.chromaticAberration;
			enabledToggle.OverrideValue(ca.active);
			intensitySlider.OverrideValue(ca.intensity.value);
			samplesSlider.OverrideValue(ca.maxSamples);
		}
	}
}
