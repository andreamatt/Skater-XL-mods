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
	public class FilmGrainHandler : PresetEffectHandler
	{
		FilmGrainUI fgUI;

		public override void ConnectUI() {
			fgUI = FilmGrainUI.Instance;

			// add listeners
			fgUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.filmGrain.active = v;
			fgUI.type.onValueChanged.AddListener(v => PresetManager.Instance.selectedPreset.filmGrain.type.value = (FilmGrainLookup)v);
			fgUI.intensity.onValueChanged += v => PresetManager.Instance.selectedPreset.filmGrain.intensity.value = v;
			fgUI.response.onValueChanged += v => PresetManager.Instance.selectedPreset.filmGrain.response.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var fg = preset.filmGrain;
			fgUI.toggle.SetIsOnWithoutNotify(fg.active);
			fgUI.type.SetValueWithoutNotify((int)fg.type.value);
			fgUI.intensity.OverrideValue(fg.intensity.value);
			fgUI.response.OverrideValue(fg.response.value);
		}
	}
}
