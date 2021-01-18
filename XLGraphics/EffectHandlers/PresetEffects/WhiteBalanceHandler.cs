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
	public class WhiteBalanceHandler : PresetEffectHandler
	{

		WhiteBalanceUI wbUI;

		public override void ConnectUI() {
			wbUI = WhiteBalanceUI.Instance;

			// add listeners
			wbUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.whiteBalance.active = v;
			wbUI.temperature.onValueChanged += v => PresetManager.Instance.selectedPreset.whiteBalance.temperature.value = v;
			wbUI.tint.onValueChanged += v => PresetManager.Instance.selectedPreset.whiteBalance.tint.value = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var wb = preset.whiteBalance;
			wbUI.toggle.SetIsOnWithoutNotify(wb.active);
			wbUI.temperature.OverrideValue(wb.temperature.value);
			wbUI.tint.OverrideValue(wb.tint.value);
		}
	}
}
