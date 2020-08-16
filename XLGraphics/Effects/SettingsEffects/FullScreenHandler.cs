using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.Effects.SettingsEffects
{
	class FullScreenHandler : EffectHandler
	{
		public override void ConnectUI() {
			var FSModeIndexToValue = new Dictionary<int, FullScreenMode> {
				[0] = FullScreenMode.ExclusiveFullScreen,
				[1] = FullScreenMode.FullScreenWindow,
				[2] = FullScreenMode.Windowed
			};
			var FSModeValueToIndex = FSModeIndexToValue.ToDictionary(kv => kv.Value, kv => kv.Key);

			var fsUI = FullScreenUI.Instance;

			// init UI values
			Screen.fullScreenMode = Main.settings.settingsData.SCREEN_MODE;
			fsUI.dropdown.SetValueWithoutNotify(FSModeValueToIndex[Main.settings.settingsData.SCREEN_MODE]);

			// add listeners
			fsUI.dropdown.onValueChanged.AddListener(new UnityAction<int>(index => {
				Screen.fullScreenMode = Main.settings.settingsData.SCREEN_MODE = FSModeIndexToValue[index];
			}));
		}
	}
}
