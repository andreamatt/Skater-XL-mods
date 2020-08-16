using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Utils;

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

			var FSmodeDropdown = UI.Instance.dropdowns["FullScreenDropdown"];

			// init UI values
			Screen.fullScreenMode = Main.settings.SCREEN_MODE;
			FSmodeDropdown.OverrideValue(FSModeValueToIndex[Main.settings.SCREEN_MODE]);

			// add listeners
			FSmodeDropdown.ValueChange += index => {
				Screen.fullScreenMode = Main.settings.SCREEN_MODE = FSModeIndexToValue[index];
			};
		}
	}
}
