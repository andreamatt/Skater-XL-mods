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
	public class PaniniProjectionHandler : PresetEffectHandler
	{
		PaniniProjectionUI paniniUI;

		public override void ConnectUI() {
			paniniUI = PaniniProjectionUI.Instance;

			// add listeners
			paniniUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.paniniProjection.active = v;
			paniniUI.distance.onValueChanged += v => PresetManager.Instance.selectedPreset.paniniProjection.distance.value = v;
			paniniUI.cropToFit.onValueChanged += v => PresetManager.Instance.selectedPreset.paniniProjection.cropToFit.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var panini = preset.paniniProjection;
			paniniUI.toggle.SetIsOnWithoutNotify(panini.active);
			paniniUI.distance.OverrideValue(panini.distance.value);
			paniniUI.cropToFit.OverrideValue(panini.cropToFit.value);
		}
	}
}
