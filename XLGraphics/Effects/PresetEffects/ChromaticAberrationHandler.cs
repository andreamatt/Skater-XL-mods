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

namespace XLGraphics.Effects.PresetEffects
{
	public class ChromaticAberrationHandler : PresetEffectHandler
	{
		ChromaticAberrationUI caUI;

		public override void ConnectUI() {
			caUI = ChromaticAberrationUI.Instance;

			// add listeners
			caUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(v => PresetManager.Instance.selectedPreset.chromaticAberration.active = v));
			caUI.intensity.onValueChange += v => PresetManager.Instance.selectedPreset.chromaticAberration.intensity.value = v;
			caUI.maxSamples.onValueChange += v => PresetManager.Instance.selectedPreset.chromaticAberration.maxSamples = (int)Math.Round(v);
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var ca = preset.chromaticAberration;
			caUI.toggle.SetIsOnWithoutNotify(ca.active);
			caUI.intensity.OverrideValue(ca.intensity.value);
			caUI.maxSamples.OverrideValue(ca.maxSamples);
		}
	}
}
